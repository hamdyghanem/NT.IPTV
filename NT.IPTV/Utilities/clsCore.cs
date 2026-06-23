using Microsoft.Win32;
using Newtonsoft.Json;
using NT.IPTV.Models;
using NT.IPTV.Models.Channel;
using NT.IPTV.Models.Items;
using NT.IPTV.Models.Items.Channesl;
using NT.IPTV.Models.Items.StreamObject;
using System;
using System.Configuration;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Net;
using System.Reflection;
using System.Security.Policy;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace NT.IPTV.Utilities
{
    public static class clsCore
    {
        private static readonly HttpClient _httpClient = CreateHttpClient();
        public static readonly string UserProfiles = "UserProfiles";
        private const string settingsFileName = "settings.json";
        public static readonly string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static CancellationTokenSource _cts = new CancellationTokenSource();
        public static string DownloadeFolder
        {
            get
            {
                var f = Path.Combine(assemblyFolder, "Downloades");
                if (!Directory.Exists(f))
                    Directory.CreateDirectory(f);
                return f;
            }
        }
        public static string CleanName(string name)
        {
            return name.Replace(":", " ").Replace("\\", " ").Replace("/", " ");
        }

        #region clsCore Data Storage
        public static enumCategories CurrentCategory { get; set; } = enumCategories.Live;
        public static string ServerConnectionString { get; set; } = string.Empty;
        public static AppSettings Config { get; set; } = new AppSettings();
        public static string PlaylistDataConnectionString { get; set; } = string.Empty;
        //User's login info to use throughout the program
        public static UserInfo currentUser = new UserInfo();

        //Contains the User_Info and Server_Info objects
        public static PlayerInfo PlayerInfo = new PlayerInfo();

        public static string allXtreamEpgData { get; set; }

        // Property to store the Xtream categories
        public static List<StreamCategory> ChannelCategories { get; set; } = new List<StreamCategory>();
        public static List<StreamCategory> MoviesCategories { get; set; } = new List<StreamCategory>();
        public static List<StreamCategory> SeriesCategories { get; set; } = new List<StreamCategory>();
        //
        public static List<StreamChannel> AllStreamChannels { get; set; } = new List<StreamChannel>();
        public static List<StreamSeries> AllStreamSerieses { get; set; } = new List<StreamSeries>();
        public static List<StreamVideo> AllStreamVideos { get; set; } = new List<StreamVideo>();

        public static List<StreamChannel> StreamChannels { get; set; } = new List<StreamChannel>();
        public static List<StreamSeries> StreamSerieses { get; set; } = new List<StreamSeries>();
        public static List<StreamVideo> StreamVideos { get; set; } = new List<StreamVideo>();

        /// <summary>True when the catalog was loaded from local cache (not from the server this session).</summary>
        public static bool CatalogLoadedFromCache { get; set; } = false;
        /// <summary>Timestamp of the currently loaded catalog (server fetch time or cache write time).</summary>
        public static DateTime CatalogTimestamp { get; set; } = DateTime.MinValue;

        #endregion

        #region Catalog Cache ...

        private static string GetCacheFolder()
        {
            var folder = Path.Combine(assemblyFolder!, "Cache", CleanName(currentUser.Name));
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            return folder;
        }

        private static string GetCacheFilePath() =>
            Path.Combine(GetCacheFolder(), "catalog.json");

        private class CatalogCache
        {
            public DateTime SavedAt { get; set; }
            public List<StreamCategory> ChannelCategories { get; set; } = new();
            public List<StreamCategory> MoviesCategories { get; set; } = new();
            public List<StreamCategory> SeriesCategories { get; set; } = new();
            public List<StreamChannel> AllStreamChannels { get; set; } = new();
            public List<StreamVideo> AllStreamVideos { get; set; } = new();
            public List<StreamSeries> AllStreamSerieses { get; set; } = new();
        }

        /// <summary>Persist the current in-memory catalog to disk for fast next-launch loading.</summary>
        public static void SaveCatalogCache()
        {
            if (!Config.EnableCatalogCache) return;
            try
            {
                var cache = new CatalogCache
                {
                    SavedAt = DateTime.UtcNow,
                    ChannelCategories = ChannelCategories,
                    MoviesCategories = MoviesCategories,
                    SeriesCategories = SeriesCategories,
                    AllStreamChannels = AllStreamChannels,
                    AllStreamVideos = AllStreamVideos,
                    AllStreamSerieses = AllStreamSerieses,
                };
                File.WriteAllText(GetCacheFilePath(),
                    JsonConvert.SerializeObject(cache, Formatting.None));
            }
            catch { /* cache write failure is non-fatal */ }
        }

        /// <summary>
        /// Try to load catalog from disk. Returns true and populates in-memory lists if the
        /// cache file exists and is still within the configured expiry window.
        /// </summary>
        public static bool TryLoadCatalogCache()
        {
            if (!Config.EnableCatalogCache) return false;
            var path = GetCacheFilePath();
            if (!File.Exists(path)) return false;
            try
            {
                var json = File.ReadAllText(path);
                var cache = JsonConvert.DeserializeObject<CatalogCache>(json);
                if (cache == null) return false;
                var age = DateTime.UtcNow - cache.SavedAt;
                if (age.TotalHours > Config.CacheExpiryHours) return false;

                ChannelCategories = cache.ChannelCategories;
                MoviesCategories  = cache.MoviesCategories;
                SeriesCategories  = cache.SeriesCategories;
                AllStreamChannels = cache.AllStreamChannels;
                AllStreamVideos   = cache.AllStreamVideos;
                AllStreamSerieses = cache.AllStreamSerieses;
                CatalogTimestamp  = cache.SavedAt;
                CatalogLoadedFromCache = true;
                return true;
            }
            catch { return false; }
        }

        /// <summary>Delete the cache file for the current profile so the next login re-fetches from server.</summary>
        public static void InvalidateCatalogCache()
        {
            try
            {
                var path = GetCacheFilePath();
                if (File.Exists(path)) File.Delete(path);
            }
            catch { }
            CatalogLoadedFromCache = false;
        }

        #endregion

        #region Image Cache ...

        public static class ImageCache
        {
            private static readonly System.Collections.Concurrent.ConcurrentDictionary<string, Bitmap> _cache = new();
            private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(8);

            public static async Task<Bitmap?> GetImageAsync(string url)
            {
                if (string.IsNullOrEmpty(url)) return null;

                if (_cache.TryGetValue(url, out var bitmap))
                    return bitmap;

                await _semaphore.WaitAsync();
                try
                {
                    // Double check after acquiring lock
                    if (_cache.TryGetValue(url, out bitmap))
                        return bitmap;

                    using var response = await _httpClient.GetAsync(GetProxiedUrl(url));
                    response.EnsureSuccessStatusCode();
                    using var stream = await response.Content.ReadAsStreamAsync();
                    var newBitmap = new Bitmap(stream);
                    _cache.TryAdd(url, newBitmap);
                    return newBitmap;
                }
                catch
                {
                    return null;
                }
                finally
                {
                    _semaphore.Release();
                }
            }

            public static void Clear()
            {
                foreach (var img in _cache.Values)
                {
                    try { img.Dispose(); } catch { }
                }
                _cache.Clear();
            }
        }

        #endregion

        #region Operations ...
        public static string GetVLCPath()
        {
            // Attempt to get the path from the configuration file
            string vlcPath = Config.VlcLocationPath;

            // Check if the path is not null/empty and if the file exists
            if (!string.IsNullOrEmpty(vlcPath) && File.Exists(vlcPath))
            {
                return vlcPath;
            }
            else
            {
                // Path is invalid or not set, find and update the path
                vlcPath = FindVLCPath();
                if (vlcPath != null)
                {
                    // Update the configuration with the found path
                    Config.VlcLocationPath = vlcPath;
                    SaveConfiguration();
                }
                return vlcPath;
            }
        }

        public static string FindVLCPath()
        {
            // Registry keys to check
            string[] registryKeys = new string[]
            {
                @"SOFTWARE\VideoLAN\VLC",
                @"SOFTWARE\WOW6432Node\VideoLAN\VLC"
            };

            foreach (var keyPath in registryKeys)
            {
                using (var key = Registry.LocalMachine.OpenSubKey(keyPath))
                {
                    if (key != null)
                    {
                        var installDir = key.GetValue("InstallDir") as string;
                        if (!string.IsNullOrEmpty(installDir))
                        {
                            return Path.Combine(installDir, "vlc.exe");
                        }
                    }
                }
            }

            return null; // VLC not found
        }

        public static void loadUsersFromDirectory(ComboBox cboProfile)
        {
            var saveDir = Path.Combine(assemblyFolder, UserProfiles);

            if (!Directory.Exists(saveDir))
                Directory.CreateDirectory(saveDir);
            DirectoryInfo DI = new DirectoryInfo(saveDir);
            FileInfo[] files = DI.GetFiles("*.json"); // Assuming user data is stored in JSON files
                                                      // Clear existing items
            if (cboProfile.Items != null)
                cboProfile.Items.Clear();
            // Add user files to the combobox, removing the file extension for display
            foreach (var file in files)
            {
                cboProfile.Items.Add(Path.GetFileNameWithoutExtension(file.Name));
            }
        }

        public static async Task<bool> CheckLoginConnection(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();

                clsCore.ServerConnectionString = $"{(clsCore.currentUser.UseHttps ? "https" : "http")}://{clsCore.currentUser.Server}";
                if (!string.IsNullOrEmpty(clsCore.currentUser.Port))
                {
                    clsCore.ServerConnectionString += $":{clsCore.currentUser.Port}";
                }
                clsCore.ServerConnectionString += $"/player_api.php?username={clsCore.currentUser.UserName}&password={clsCore.currentUser.Password}";
                clsCore.ServerConnectionString = clsCore.ServerConnectionString.Replace("http://http://", "http://").Replace("https://https://", "https://");
                //
                clsCore.PlaylistDataConnectionString = clsCore.ServerConnectionString.Replace("player_api.php", "xmltv.php");
                // Create a request for the URL.
                WebRequest request = WebRequest.Create(GetProxiedUrl(clsCore.ServerConnectionString));

                // If required by the server, set the credentials.
                request.Credentials = CredentialCache.DefaultCredentials;

                // Get the response.
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();

                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = await reader.ReadToEndAsync();
                // Display the content.
                Debug.WriteLine("Response from server: " + responseFromServer);

                PlayerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<PlayerInfo>(responseFromServer);
                if (info.user_info.status == "Expired")
                {
                    MessageBox.Show("Account is expired");
                    return false;
                }
                clsCore.PlayerInfo = info;

                // Cleanup the streams and the response.
                reader.Close();
                dataStream.Close();
                response.Close();

                return true; // Connection was successful
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse response = (HttpWebResponse)ex.Response;
                    using (Stream errorResponse = response.GetResponseStream())
                    {
                        if (errorResponse != null)
                        {
                            StreamReader errorReader = new StreamReader(errorResponse);
                            string errorResponseText = await errorReader.ReadToEndAsync();
                            // Display the content without HTML tags.
                            string textOnly = System.Text.RegularExpressions.Regex.Replace(errorResponseText, "<.*?>", "");
                            MessageBox.Show("Response from server: " + textOnly);
                        }
                    }
                }
                MessageBox.Show("Error: " + ex.Message);

                return false; // Connection was not successful
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Login Canceled.");
                return false;
            }
        }

        public static async Task RetrieveCategories(ToolStripStatusLabel lblStatus, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                //
                lblStatus.Text = "Loading live channels categories";
                var url = $"{clsCore.ServerConnectionString}&action=get_live_categories";
                HttpResponseMessage response = await _httpClient.GetAsync(GetProxiedUrl(url), token);
                response.EnsureSuccessStatusCode();
                var responseFromServer = await response.Content.ReadAsStringAsync();
                ChannelCategories = JsonConvert.DeserializeObject<StreamCategory[]>(responseFromServer).ToList();
                ChannelCategories.Where(c => currentUser.FavoritChannelsCategory.Contains(c.ID)).All(x => x.Favorite = true);
                //Add Favorites
                ChannelCategories.Insert(0, new StreamCategory { ID = "-1", Name = "Favorites", Favorite = true });
                //
                lblStatus.Text = "Loading videos categories";
                url = $"{clsCore.ServerConnectionString}&action=get_vod_categories";
                response = await _httpClient.GetAsync(GetProxiedUrl(url), token);
                response.EnsureSuccessStatusCode();
                responseFromServer = await response.Content.ReadAsStringAsync();
                MoviesCategories = JsonConvert.DeserializeObject<StreamCategory[]>(responseFromServer).ToList();
                MoviesCategories.Where(c => currentUser.FavoritMoviesCategory.Contains(c.ID)).All(x => x.Favorite = true);
                MoviesCategories.Insert(0, new StreamCategory { ID = "-1", Name = "Favorites", Favorite = true });
                //
                lblStatus.Text = "Loading series categories";
                url = $"{clsCore.ServerConnectionString}&action=get_series_categories";
                response = await _httpClient.GetAsync(GetProxiedUrl(url), token);
                response.EnsureSuccessStatusCode();
                responseFromServer = await response.Content.ReadAsStringAsync();
                SeriesCategories = JsonConvert.DeserializeObject<StreamCategory[]>(responseFromServer).ToList();
                SeriesCategories.Where(c => currentUser.FavoritSeriesCategory.Contains(c.ID)).All(x => x.Favorite = true);
                SeriesCategories.Insert(0, new StreamCategory { ID = "-1", Name = "Favorites", Favorite = true });
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show("HTTP Error: " + ex.Message);
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Operation canceled.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
        public static async Task RetrieveStreams(StreamCategory selectedItem, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                switch (clsCore.CurrentCategory)
                {
                    case enumCategories.Live:
                        {
                            if (selectedItem.ID == "-1")
                            {
                                StreamChannels = AllStreamChannels.Where(c => currentUser.FavoritChannels.Contains(c.ID)).ToList();
                                StreamChannels.All(c => c.Favorite = true);
                            }
                            else
                            {
                                //if (AllStreamChannels.Count > 0)
                                //{
                                StreamChannels = AllStreamChannels.Where(x => x.CategoryID == selectedItem.ID).ToList();
                                //}
                                //else
                                //{
                                //    var url = $"{clsCore.ServerConnectionString}&action=get_live_streams&category_id={selectedItem.ID}";
                                //    var response = await _httpClient.GetAsync(url, token);
                                //    response.EnsureSuccessStatusCode(); // Throw if not a success code.
                                //    var responseFromServer = await response.Content.ReadAsStringAsync();
                                //    clsCore.StreamChannels = JsonConvert.DeserializeObject<StreamChannel[]>(responseFromServer).ToList();
                                //}
                                StreamChannels.Where(c => currentUser.FavoritChannels.Contains(c.ID)).All(x => x.Favorite = true);
                            }
                        }
                        break;
                    case enumCategories.Movies:
                        {
                            if (selectedItem.ID == "-1")
                            {
                                StreamVideos = AllStreamVideos.Where(c => currentUser.FavoritMovies.Contains(c.ID)).ToList();

                            }
                            else
                            {
                                //if (AllStreamVideos.Count > 0)
                                //{
                                StreamVideos = AllStreamVideos.Where(x => x.CategoryID == selectedItem.ID).ToList();
                                //}
                                //else
                                //{
                                //    var url = $"{clsCore.ServerConnectionString}&action=get_vod_streams&category_id={selectedItem.ID}";
                                //    var response = await _httpClient.GetAsync(url, token);
                                //    response.EnsureSuccessStatusCode(); // Throw if not a success code.
                                //    var responseFromServer = await response.Content.ReadAsStringAsync();
                                //    clsCore.StreamVideos = JsonConvert.DeserializeObject<StreamVideo[]>(responseFromServer).ToList();
                                //}
                                StreamVideos.Where(c => currentUser.FavoritMovies.Contains(c.ID)).All(x => x.Favorite = true);
                            }
                        }
                        break;
                    case enumCategories.Series:
                        {
                            if (selectedItem.ID == "-1")
                            {
                                StreamSerieses = AllStreamSerieses.Where(c => currentUser.FavoritSeries.Contains(c.ID)).ToList();
                                StreamSerieses.All(c => c.Favorite = true);
                                foreach(StreamSeries s in  StreamSerieses)
                                {
                                    var x = await clsCore.GetSeriesInfo(s.StreamID,clsCore. _cts.Token);
                                    if (x.HasNewEpisodes)
                                    {
                                        s.HasNewEpisodes = true;    
                                    }
                                }
                            }
                            else
                            {
                                //if (AllStreamSerieses.Count > 0)
                                //{
                                StreamSerieses = AllStreamSerieses.Where(x => x.CategoryID == selectedItem.ID).ToList();
                                //}
                                //else
                                //{
                                //    var url = $"{clsCore.ServerConnectionString}&action=get_series&category_id={selectedItem.ID}";
                                //    var response = await _httpClient.GetAsync(url, token);
                                //    response.EnsureSuccessStatusCode(); // Throw if not a success code.
                                //    var responseFromServer = await response.Content.ReadAsStringAsync();
                                //    StreamSerieses = JsonConvert.DeserializeObject<StreamSeries[]>(responseFromServer).ToList();
                                //}
                                //load favorites
                                StreamSerieses.Where(c => currentUser.FavoritSeries.Contains(c.ID)).All(x => x.Favorite = true);
                            }
                        }
                        break;
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show("HTTP Error: " + ex.Message);
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Operation canceled.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
        public static async Task RetrieveStreams(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                {
                    switch (clsCore.CurrentCategory)
                    {
                        case enumCategories.Live:
                            {
                                //lblStatus?.Text = "Loading live streams";
                                if (AllStreamChannels.Count == 0)
                                {
                                    var url = $"{clsCore.ServerConnectionString}&action=get_live_streams";
                                    var response = await _httpClient.GetAsync(GetProxiedUrl(url), token);
                                    response.EnsureSuccessStatusCode();
                                    var responseFromServer = await response.Content.ReadAsStringAsync();
                                    AllStreamChannels = JsonConvert.DeserializeObject<StreamChannel[]>(responseFromServer).ToList();
                                }
                                break;
                            }

                        case enumCategories.Movies:
                            {
                                //lblStatus.Text = "Loading movies streams";
                                if (AllStreamVideos.Count == 0)
                                {
                                    var url = $"{clsCore.ServerConnectionString}&action=get_vod_streams";
                                    var response = await _httpClient.GetAsync(GetProxiedUrl(url), token);
                                    response.EnsureSuccessStatusCode();
                                    var responseFromServer = await response.Content.ReadAsStringAsync();
                                    AllStreamVideos = JsonConvert.DeserializeObject<StreamVideo[]>(responseFromServer).ToList();
                                }
                                break;
                            }

                        //
                        case enumCategories.Series:
                            {
                                //lblStatus.Text = "Loading series streams";
                                if (clsCore.AllStreamSerieses.Count == 0)
                                {
                                    var url = $"{clsCore.ServerConnectionString}&action=get_series";
                                    var response = await _httpClient.GetAsync(GetProxiedUrl(url), token);
                                    response.EnsureSuccessStatusCode();
                                    var responseFromServer = await response.Content.ReadAsStringAsync();
                                    clsCore.AllStreamSerieses = JsonConvert.DeserializeObject<StreamSeries[]>(responseFromServer).ToList();
                                }
                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
        public static async Task<WatchMovie> GetMovieInfo(string vodID, CancellationToken token)
        {
            try
            {
                var url = $"{clsCore.ServerConnectionString}&action=get_vod_info&vod_id={vodID}";
                var response = await _httpClient.GetAsync(GetProxiedUrl(url), token);
                response.EnsureSuccessStatusCode();
                var responseFromServer = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<WatchMovie>(responseFromServer);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public static async Task<WatchSeries> GetSeriesInfo(string seriesID, CancellationToken token)
        {
            try
            {
                var url = $"{clsCore.ServerConnectionString}&action=get_series_info&series_id={seriesID}";
                var response = await _httpClient.GetAsync(GetProxiedUrl(url), token);
                response.EnsureSuccessStatusCode();
                var responseFromServer = await response.Content.ReadAsStringAsync();
                var series = JsonConvert.DeserializeObject<WatchSeries>(responseFromServer);
                //get season
                for (int i = 1; i <= 15; i++)
                {
                    var d = GetPropValue(series.Episodes, $"EpisodeData_{i}");
                    if (d == null) break;
                    series.AddSeason(new Season { SeasonNum = i, Episodes = (List<EpisodeData>)d });
                }
                return series;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName)?.GetValue(src, null);
        }

        public static UserInfo GetUserData(string userName)
        {
            string filePath = Path.Combine(assemblyFolder, UserProfiles, userName + ".json");

            if (!File.Exists(filePath))
            {
                MessageBox.Show("User data file not found.");
                return null;
            }

            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<UserInfo>(json);
        }


        public static string GetSafeFilename(string filename)
        {
            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        }
        private static HttpClient CreateHttpClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.0.0 Safari/537.36");
            return client;
        }

        /// <summary>
        /// When UseAzureProxy is enabled, wraps <paramref name="originalUrl"/> as
        /// {ProxyBaseUrl}/proxy?url={encoded} so all traffic exits through Azure
        /// (bypassing ISP blocks on the IPTV provider's IP).
        /// </summary>
        public static string GetProxiedUrl(string originalUrl)
        {
            if (Config.UseAzureProxy && !string.IsNullOrWhiteSpace(originalUrl))
            {
                // Avoid Cloudflare/CDN blocking of cloud IPs (Azure) for external poster assets (e.g. TMDB, Imgur).
                // Only proxy requests that belong to our IPTV server domain.
                var serverHost = currentUser?.Server;
                bool shouldProxy = true;

                if (!string.IsNullOrEmpty(serverHost))
                {
                    if (originalUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase) &&
                        originalUrl.IndexOf(serverHost, StringComparison.OrdinalIgnoreCase) == -1)
                    {
                        shouldProxy = false; // Bypass proxy for external domains (e.g., tmdb.org)
                    }
                }

                if (shouldProxy)
                {
                    var baseUrl = Config.ProxyBaseUrl.TrimEnd('/');
                    return baseUrl + "/proxy?url=" + Uri.EscapeDataString(originalUrl);
                }
            }
            return originalUrl;
        }
        /// <summary>
        /// Central playback dispatcher. Opens the built-in <see cref="frmPlayMovie"/> when
        /// <see cref="AppSettings.UseBuiltInPlayer"/> is <c>true</c>; otherwise spawns VLC externally.
        /// </summary>
        /// <param name="streamUrl">The raw stream URL (proxying is handled internally).</param>
        public static void PlayStream(string streamUrl)
        {
            if (string.IsNullOrWhiteSpace(streamUrl))
            {
                MessageBox.Show("Stream URL not available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Config.UseBuiltInPlayer)
            {
                // Use the built-in WebView2/VLCSharp player form
                using var frm = new frmPlayMovie(streamUrl);
                frm.ShowDialog();
            }
            else
            {
                // Launch VLC externally with the (possibly proxied) stream URL
                var vlcPath = GetVLCPath();
                if (string.IsNullOrEmpty(vlcPath) || !File.Exists(vlcPath))
                {
                    MessageBox.Show(
                        "VLC Media Player was not found. Please install VLC or configure its path in settings.",
                        "VLC Not Found",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    var proxiedUrl = GetProxiedUrl(streamUrl);
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = vlcPath,
                        Arguments = $"\"{proxiedUrl}\"",
                        UseShellExecute = false
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to launch VLC: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ── Theme colours ─────────────────────────────────────────────────────────
        private static readonly Color DarkBackground  = Color.FromArgb(28,  28,  28);
        private static readonly Color DarkSurface     = Color.FromArgb(40,  40,  40);
        private static readonly Color DarkForeground  = Color.FromArgb(220, 220, 220);
        private static readonly Color DarkAccent      = Color.FromArgb(0,   120, 212);
        private static readonly Color DarkBorder      = Color.FromArgb(60,  60,  60);

        private static readonly Color LightBackground = SystemColors.Control;
        private static readonly Color LightSurface    = SystemColors.Window;
        private static readonly Color LightForeground = SystemColors.ControlText;

        /// <summary>
        /// Recursively applies the current dark / light theme to every control on
        /// <paramref name="form"/> (and to <paramref name="form"/> itself).
        /// Call this in each form's constructor or <c>Load</c> handler, and whenever
        /// <see cref="AppSettings.DarkMode"/> changes at runtime.
        /// </summary>
        public static void ApplyTheme(Form form)
        {
            bool dark = Config.DarkMode;

            // Apply to the form window itself
            form.BackColor = dark ? DarkBackground : LightBackground;
            form.ForeColor = dark ? DarkForeground  : LightForeground;

            ApplyThemeToControls(form.Controls, dark);
        }

        private static void ApplyThemeToControls(Control.ControlCollection controls, bool dark)
        {
            foreach (Control ctrl in controls)
            {
                switch (ctrl)
                {
                    case MenuStrip ms:
                        ms.BackColor = dark ? DarkSurface    : SystemColors.MenuBar;
                        ms.ForeColor = dark ? DarkForeground : SystemColors.MenuText;
                        foreach (ToolStripItem item in ms.Items)
                            ApplyThemeToToolStripItem(item, dark);
                        break;

                    case StatusStrip ss:
                        ss.BackColor = dark ? DarkSurface    : SystemColors.Control;
                        ss.ForeColor = dark ? DarkForeground : SystemColors.ControlText;
                        foreach (ToolStripItem item in ss.Items)
                            ApplyThemeToToolStripItem(item, dark);
                        break;

                    case ToolStrip ts:
                        ts.BackColor = dark ? DarkSurface    : SystemColors.Control;
                        ts.ForeColor = dark ? DarkForeground : SystemColors.ControlText;
                        foreach (ToolStripItem item in ts.Items)
                            ApplyThemeToToolStripItem(item, dark);
                        break;

                    case DataGridView dgv:
                        dgv.BackgroundColor          = dark ? DarkBackground : SystemColors.Window;
                        dgv.GridColor                = dark ? DarkBorder     : SystemColors.ControlDark;
                        dgv.DefaultCellStyle.BackColor    = dark ? DarkSurface    : SystemColors.Window;
                        dgv.DefaultCellStyle.ForeColor    = dark ? DarkForeground : SystemColors.ControlText;
                        dgv.DefaultCellStyle.SelectionBackColor = dark ? DarkAccent : SystemColors.Highlight;
                        dgv.ColumnHeadersDefaultCellStyle.BackColor = dark ? DarkSurface    : SystemColors.Control;
                        dgv.ColumnHeadersDefaultCellStyle.ForeColor = dark ? DarkForeground : SystemColors.ControlText;
                        break;

                    case ListView lv:
                        lv.BackColor = dark ? DarkSurface    : SystemColors.Window;
                        lv.ForeColor = dark ? DarkForeground : SystemColors.ControlText;
                        break;

                    case TreeView tv:
                        tv.BackColor = dark ? DarkSurface    : SystemColors.Window;
                        tv.ForeColor = dark ? DarkForeground : SystemColors.ControlText;
                        break;

                    case ListBox lb:
                        lb.BackColor = dark ? DarkSurface    : SystemColors.Window;
                        lb.ForeColor = dark ? DarkForeground : SystemColors.ControlText;
                        break;

                    case ComboBox cbo:
                        cbo.BackColor = dark ? DarkSurface    : SystemColors.Window;
                        cbo.ForeColor = dark ? DarkForeground : SystemColors.ControlText;
                        break;

                    case TextBox tb:
                        tb.BackColor = dark ? DarkSurface    : SystemColors.Window;
                        tb.ForeColor = dark ? DarkForeground : SystemColors.ControlText;
                        break;

                    case RichTextBox rtb:
                        rtb.BackColor = dark ? DarkSurface    : SystemColors.Window;
                        rtb.ForeColor = dark ? DarkForeground : SystemColors.ControlText;
                        break;

                    case Button btn:
                        btn.FlatStyle = dark ? FlatStyle.Flat : FlatStyle.Standard;
                        btn.BackColor = dark ? DarkSurface    : SystemColors.Control;
                        btn.ForeColor = dark ? DarkForeground : SystemColors.ControlText;
                        if (dark)
                        {
                            btn.FlatAppearance.BorderColor = DarkBorder;
                            btn.FlatAppearance.MouseOverBackColor = DarkAccent;
                        }
                        break;

                    case CheckBox chk:
                        chk.BackColor = dark ? DarkBackground : LightBackground;
                        chk.ForeColor = dark ? DarkForeground : LightForeground;
                        break;

                    case RadioButton rb:
                        rb.BackColor = dark ? DarkBackground : LightBackground;
                        rb.ForeColor = dark ? DarkForeground : LightForeground;
                        break;

                    case Label lbl:
                        lbl.BackColor = Color.Transparent;
                        lbl.ForeColor = dark ? DarkForeground : LightForeground;
                        break;

                    case Panel pnl:
                        pnl.BackColor = dark ? DarkBackground : LightBackground;
                        pnl.ForeColor = dark ? DarkForeground : LightForeground;
                        break;

                    case GroupBox gb:
                        gb.BackColor = dark ? DarkBackground : LightBackground;
                        gb.ForeColor = dark ? DarkForeground : LightForeground;
                        break;

                    case TabControl tc:
                        tc.BackColor = dark ? DarkBackground : LightBackground;
                        tc.ForeColor = dark ? DarkForeground : LightForeground;
                        foreach (TabPage tp in tc.TabPages)
                        {
                            tp.BackColor = dark ? DarkBackground : LightBackground;
                            tp.ForeColor = dark ? DarkForeground : LightForeground;
                            ApplyThemeToControls(tp.Controls, dark);
                        }
                        break;

                    case SplitContainer sc:
                        sc.BackColor = dark ? DarkBackground : LightBackground;
                        ApplyThemeToControls(sc.Panel1.Controls, dark);
                        ApplyThemeToControls(sc.Panel2.Controls, dark);
                        break;

                    default:
                        // Fallback: colour any remaining control
                        try
                        {
                            ctrl.BackColor = dark ? DarkBackground : LightBackground;
                            ctrl.ForeColor = dark ? DarkForeground : LightForeground;
                        }
                        catch { /* Some controls (e.g. PictureBox) may reject colour changes */ }
                        break;
                }

                // Recurse into child controls (skipping TabControl/SplitContainer already handled above)
                if (ctrl is not TabControl && ctrl is not SplitContainer && ctrl.HasChildren)
                    ApplyThemeToControls(ctrl.Controls, dark);
            }
        }

        private static void ApplyThemeToToolStripItem(ToolStripItem item, bool dark)
        {
            item.BackColor = dark ? DarkSurface    : SystemColors.Control;
            item.ForeColor = dark ? DarkForeground : SystemColors.ControlText;
            if (item is ToolStripDropDownItem ddi)
                foreach (ToolStripItem sub in ddi.DropDownItems)
                    ApplyThemeToToolStripItem(sub, dark);
        }

        #endregion


        #region Global helpers
        public static string SelectedCategory { get; set; } = string.Empty;


        public static bool ShouldUpdateOnInterval(DateTime currentTime) // make this a settings config eventually maybe?
        {
            return currentTime.Minute % 15 == 0 && currentTime.Second == 0;
        }
        public static async Task<bool> Check404(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine(response.StatusCode.ToString());
                    }
                    else
                    {
                        //response.StatusCode
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async void dotest(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode.ToString());
                }
                else
                {
                    // problems handling here
                    Console.WriteLine(
                        "Error occurred, the status code is: {0}",
                        response.StatusCode
                    );
                }
            }
        }

        #endregion

        #region Settings ...
        public static void SaveConfiguration()
        {
            string filePath = Path.Combine(assemblyFolder, settingsFileName);
            string json = JsonConvert.SerializeObject(clsCore.Config, Formatting.Indented);
            File.WriteAllText(filePath, json);
            //
            filePath = Path.Combine(assemblyFolder, UserProfiles, currentUser.Name + ".json");
            json = JsonConvert.SerializeObject(clsCore.currentUser, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static void LoadConfiguration()
        {
            var saveDir = Path.Combine(assemblyFolder, UserProfiles);
            string filePath = Path.Combine(assemblyFolder, settingsFileName);
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                Config = JsonConvert.DeserializeObject<AppSettings>(json);
            }
            else
            {
                Config = new AppSettings();
            }
        }
        #endregion
    }

}