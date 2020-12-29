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
using your_grs_actionsTracker.DataDownload;
using your_grs_actionsTracker.InstaSharperActions;

namespace your_grs_actionsTracker
{
    class Program
    {
        /// <summary>
        ///     Api instance (one instance per Instagram user)
        /// </summary>
        private static IInstaApi _instaApi;
        private static Dictionary<string, List<string>> URI = new Dictionary<string, List<string>>()
        {
            ["photoURI"] = new List<string>(),
            ["videoURI"] = new List<string>()
        };
        static async Task Main(string[] args)
        {
            var result = Task.Run(MainAsync).GetAwaiter().GetResult();
            if (result)
                return;
            Console.WriteLine("Вход в аккаунт выполнен успешно.");
            Console.WriteLine("Выберите дальнейшее действие: " +
                "\n1 - Получить медиа-ресурсы с профиля");
            var pressed = Console.ReadLine();
            switch (pressed)
            {
                case "1":
                    var parsing = new Parsing(_instaApi);
                    Console.WriteLine("Введите id пользователя: ");
                    var objectId = Console.ReadLine();
                    // GETTING MEDIA INFORMATION(URISES).
                    URI = await parsing.GetMedia(objectId, URI);
                    Console.WriteLine("Данные о медиа-ресурсах получены. Скачать ресурсы?(Y - да, N - нет)");
                    break;
                default:
                    break;
            }
            pressed = Console.ReadLine();
            switch (pressed)
            {
                case "Y":
                    Downloader.DownloadPhotoToFolder(URI);
                    Downloader.DownloadVideoToFolder(URI);
                    Console.WriteLine("Данные успешно загружены.");
                    break;
                default:
                    break;
            }
            Console.ReadLine();
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
