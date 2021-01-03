using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using InstaParseWPF.InstagramTracker;

namespace InstaParseWPF.InstagramTracker
{
    static class Downloader
    {
        public async static Task DownloadPhotoToFolder(string pathToSave, string userName, Dictionary<string, List<string>> URI)
        {
            pathToSave += @"\" + userName + @"\Photo";
            var dirInfo = new DirectoryInfo(pathToSave);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            var temporaryCounter = 1;
            var tasks = new List<Task>();
            for (int i = 0; i < URI["photoURI"].Count; i++)
            {
                using (WebClient wc = new WebClient())
                {
                    tasks.Add(wc.DownloadFileTaskAsync(new Uri(URI["photoURI"][i]), $@"{pathToSave}\\{temporaryCounter}.jpg"));
                    temporaryCounter += 1;
                }
            }
            await Task.WhenAll(tasks);
        }
        public async static Task DownloadVideoToFolder(string pathToSave, string userName, Dictionary<string, List<string>> URI)
        {
            pathToSave += @"\" + userName + @"\Video";
            var dirInfo = new DirectoryInfo(pathToSave);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            var temporaryCounter = 1;
            var tasks = new List<Task>();
            for (int i = 0; i < URI["videoURI"].Count; i++)
            {
                using (WebClient wc = new WebClient())
                {
                    tasks.Add(wc.DownloadFileTaskAsync(new Uri(URI["videoURI"][i]), $@"{pathToSave}\\{temporaryCounter}video.mp4"));
                    temporaryCounter += 1;
                }
            }
            await Task.WhenAll(tasks);
        }
    }
}
