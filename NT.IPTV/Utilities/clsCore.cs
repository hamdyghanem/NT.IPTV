using Microsoft.Win32;
using Newtonsoft.Json;
using NT.IPTV.Models;
using NT.IPTV.Models.Channel;
using NT.IPTV.Models.StreamObject;
using System.Configuration;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Net;
using System.Reflection;

namespace NT.IPTV.Utilities
{
    public static class clsCore
    {
        private static readonly HttpClient _httpClient = CreateHttpClient();
        public static readonly string UserProfiles = "UserProfiles";
        private const string settingsFileName = "settings.json";
        public static readonly string DownloadeFolder = "Downloades";
        public static readonly string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

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
        public static List<StreamChannel> StreamChannels { get; set; } = new List<StreamChannel>();
        public static List<StreamSeries> StreamSerieses { get; set; } = new List<StreamSeries>();
        public static List<StreamVideo> StreamVideos { get; set; } = new List<StreamVideo>();

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
                WebRequest request = WebRequest.Create(clsCore.ServerConnectionString);

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

        public static async Task RetrievePlaylistData(ToolStripStatusLabel lbl, CancellationToken token)//maybe pass in the action as a string and use this for all action calls
        {
            try
            {
                token.ThrowIfCancellationRequested();

                //string url = $"{(clsCore.currentUser.UseHttps ? "https" : "http")}://{clsCore.currentUser.Server}:{clsCore.currentUser.Port}/player_api.php?username={clsCore.currentUser.UserName}&password={clsCore.currentUser.Password}&action=get_live_streams";
                string url = $"{clsCore.ServerConnectionString}&action=get_live_streams";
                clsCore.StreamChannels.Clear();
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(url, token);
                    response.EnsureSuccessStatusCode(); // Throw if not a success code.

                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON response into an array of StreamChannel objects.
                    var channels = JsonConvert.DeserializeObject<StreamChannel[]>(responseContent);

                    clsCore.StreamChannels.AddRange(channels);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show("HTTP Error: " + ex.Message);
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Login Canceled.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        //done, rewritten
        public static async Task RetrieveCategories(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();

                switch (clsCore.CurrentCategory)
                {
                    case enumCategories.Live:
                        {
                            if (ChannelCategories.Count == 0)
                            {
                                var url = $"{clsCore.ServerConnectionString}&action=get_live_categories";
                                HttpResponseMessage response = await _httpClient.GetAsync(url, token);
                                response.EnsureSuccessStatusCode(); // Throw if not a success code.
                                var responseFromServer = await response.Content.ReadAsStringAsync();
                                ChannelCategories = JsonConvert.DeserializeObject<StreamCategory[]>(responseFromServer).ToList();
                                ChannelCategories.All(x => x.Favorite = Config.FavoritChannelsCategory.Contains(x.ID));
                                Config.FavoritChannelsCategory.All(x => ChannelCategories.Single(c => x == c.ID).Favorite = true);
                            }
                        }
                        break;
                    case enumCategories.Movies:
                        {
                            if (MoviesCategories.Count == 0)
                            {
                                var url = $"{clsCore.ServerConnectionString}&action=get_vod_categories";
                                var response = await _httpClient.GetAsync(url, token);
                                response.EnsureSuccessStatusCode(); // Throw if not a success code.
                                var responseFromServer = await response.Content.ReadAsStringAsync();
                                MoviesCategories = JsonConvert.DeserializeObject<StreamCategory[]>(responseFromServer).ToList();
                                Config.FavoritMoviesCategory.All(x => MoviesCategories.Single(c => x == c.ID).Favorite = true);
                            }
                        }
                        break;
                    case enumCategories.Series:
                        {
                            if (SeriesCategories.Count == 0)
                            {
                                var url = $"{clsCore.ServerConnectionString}&action=get_series_categories";
                                var response = await _httpClient.GetAsync(url, token);
                                response.EnsureSuccessStatusCode(); // Throw if not a success code.
                                var responseFromServer = await response.Content.ReadAsStringAsync();
                                SeriesCategories = JsonConvert.DeserializeObject<StreamCategory[]>(responseFromServer).ToList();
                                Config.FavoritSeriesCategory.All(x => SeriesCategories.Single(c => x == c.ID).Favorite = true);
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
        public static async Task RetrieveStreams(StreamCategory selectedItem, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                switch (clsCore.CurrentCategory)
                {
                    case enumCategories.Live:
                        {
                            var url = $"{clsCore.ServerConnectionString}&action=get_live_streams&category_id={selectedItem.ID}";
                            var response = await _httpClient.GetAsync(url, token);
                            response.EnsureSuccessStatusCode(); // Throw if not a success code.
                            var responseFromServer = await response.Content.ReadAsStringAsync();
                            clsCore.StreamChannels = JsonConvert.DeserializeObject<StreamChannel[]>(responseFromServer).ToList();
                        }
                        break;
                    case enumCategories.Movies:
                        {
                            var url = $"{clsCore.ServerConnectionString}&action=get_vod_streams&category_id={selectedItem.ID}";
                            var response = await _httpClient.GetAsync(url, token);
                            response.EnsureSuccessStatusCode(); // Throw if not a success code.
                            var responseFromServer = await response.Content.ReadAsStringAsync();
                            clsCore.StreamVideos = JsonConvert.DeserializeObject<StreamVideo[]>(responseFromServer).ToList();
                        }
                        break;
                    case enumCategories.Series:
                        {
                            var url = $"{clsCore.ServerConnectionString}&action=get_series&category_id={selectedItem.ID}";
                            var response = await _httpClient.GetAsync(url, token);
                            response.EnsureSuccessStatusCode(); // Throw if not a success code.
                            var responseFromServer = await response.Content.ReadAsStringAsync();
                            clsCore.StreamSerieses = JsonConvert.DeserializeObject<StreamSeries[]>(responseFromServer).ToList();
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
        public static async Task<WatchMovie> GetMovieInfo(string vodID, CancellationToken token)
        {
            try
            {
                var url = $"{clsCore.ServerConnectionString}&action=get_vod_info&vod_id={vodID}";
                var response = await _httpClient.GetAsync(url, token);
                response.EnsureSuccessStatusCode(); // Throw if not a success code.
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
                var response = await _httpClient.GetAsync(url, token);
                response.EnsureSuccessStatusCode(); // Throw if not a success code.
                var responseFromServer = await response.Content.ReadAsStringAsync();
                var series = JsonConvert.DeserializeObject<WatchSeries>(responseFromServer);
                //get season
                for (int i = 1; i <= 12; i++)
                {
                    var d = GetPropValue(series.Episodes, $"EpisodeData_{i}");
                    if (d == null) break;
                    series.seasonsData.Add(new Season { Episodes = (List<EpisodeData>)d });
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
        #endregion


        #region Global helpers
        public static string SelectedCategory { get; set; } = string.Empty;


        public static bool ShouldUpdateOnInterval(DateTime currentTime) // make this a settings config eventually maybe?
        {
            return currentTime.Minute % 15 == 0 && currentTime.Second == 0;
        }
        public static async Task<bool> Check404(string url)
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
        }

        public static void LoadConfiguration()
        {
            string filePath = Path.Combine(assemblyFolder, settingsFileName);
            string json = File.ReadAllText(filePath);
            Config = JsonConvert.DeserializeObject<AppSettings>(json);
        }
        #endregion
    }

}