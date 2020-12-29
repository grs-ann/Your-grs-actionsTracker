using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using InstaSharper.API;
using InstaSharper.API.Builder;
using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using InstaSharper.Logger;

namespace your_grs_actionsTracker
{
    class Program
    {
        /// <summary>
        ///     Api instance (one instance per Instagram user)
        /// </summary>
        private static IInstaApi _instaApi;
        private static List<string> urises = new List<string>();
        static void Main(string[] args)
        {
            var result = Task.Run(MainAsync).GetAwaiter().GetResult();
            if (result)
                return;
            Console.WriteLine("Чтобы сохранить картинки, нажмите y");
            var pressed = Console.ReadKey().KeyChar;
            if (pressed == 'y' || pressed == 'Y')
            {
                var counter = 1;
                foreach (var uri in urises)
                {
                    using (WebClient wc = new WebClient())
                    {
                        wc.DownloadFileAsync(
                            new Uri(uri),
                            $@"D:\\InstagramParse\\{counter}.jpg");
                    }
                    counter += 1;
                }
            }
            else
            {
                Console.WriteLine("GG");
            }
            Console.ReadKey();
        }
        public static async Task<bool> MainAsync()
        {
            try
            {
                WebClient client = new WebClient();
                Console.WriteLine("Starting instaSharper");
                // create user session data and provide login details.
                var userSession = new UserSessionData
                {
                    UserName = "books_online366",
                    Password = "a89879000520"
                };
                var delay = RequestDelay.FromSeconds(2, 2);
                // create new InstaApi instance using Builder
                _instaApi = InstaApiBuilder.CreateBuilder()
                    .SetUser(userSession)
                    .UseLogger(new DebugLogger(LogLevel.Exceptions)) // use logger for requests and debug messages
                    .SetRequestDelay(delay)
                    .Build();
                // authentification.
                if (!_instaApi.IsUserAuthenticated)
                {
                    // login
                    Console.WriteLine($"Logging in as {userSession.UserName}");
                    delay.Disable();
                    var logInResult = await _instaApi.LoginAsync();
                    delay.Enable();
                    if (!logInResult.Succeeded)
                    {
                        Console.WriteLine($"Unable to login: {logInResult.Info.Message}");
                        return false;
                    }
                }
                IResult<InstaMediaList> media = await _instaApi.GetUserMediaAsync("crucian.sl", PaginationParameters.Empty);
                Environment.ExpandEnvironmentVariables(@"D:\InstagramParse");
                foreach (var item in media.Value)
                {
                    Console.WriteLine(item.Images.Count);
                    foreach (var image in item.Images)
                    {
                        var uri = image.URI;
                        urises.Add(uri);
                    }
                }
 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                // perform that if user needs to logged out
                // var logoutResult = Task.Run(() => _instaApi.LogoutAsync()).GetAwaiter().GetResult();
                // if (logoutResult.Succeeded) Console.WriteLine("Logout succeed");
            }
            return false;
        }
 
    }
}
