using NT.IPTV.Models;
using NT.IPTV.Models.Channel;

namespace NT.IPTV.Utilities
{
    public static class clsCore
    {

        #region clsCore Data Storage
        public static enumCategories CurrentCategory { get; set; } = enumCategories.Live;
        public static string ServerConnectionString { get; set; } = string.Empty;
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
    }

}