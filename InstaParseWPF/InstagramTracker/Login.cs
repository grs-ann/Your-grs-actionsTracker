﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Security;
using System.Threading.Tasks;
using InstaSharper.API;
using InstaSharper.API.Builder;
using InstaSharper.Classes;
using InstaSharper.Logger;

namespace InstaParseWPF.InstagramTracker
{
    class Login
    {
        /// <summary>
        ///     Api instance (one instance per Instagram user)
        /// </summary>
        private static IInstaApi _instaApi;
        public static string userLogin { get; set; }
        public static string userPassword { get; set; }
        private static Dictionary<string, List<string>> URI = new Dictionary<string, List<string>>()
        {
            ["photoURI"] = new List<string>(),
            ["videoURI"] = new List<string>()
        };
        public static async Task<bool> MainAsync()
        {
            try
            {
                WebClient client = new WebClient();
                Console.WriteLine("Starting instaSharper");
                // create user session data and provide login details.
                var userSession = new UserSessionData
                {
                    UserName = userLogin,
                    Password = userPassword
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
                    else
                    {
                        return true;
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