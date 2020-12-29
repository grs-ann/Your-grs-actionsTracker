using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using your_grs_actionsTracker.InstaSharperActions;

namespace your_grs_actionsTracker.DataDownload
{
    static class Downloader
    {
        public static void DownloadPhotoToFolder(Dictionary<string, List<string>> URI)
        {
            using (WebClient wc = new WebClient())
            {
                var temporaryCounter = 1;
                for (int i = 0; i < URI["photoURI"].Count; i++)
                {
                    wc.DownloadFileAsync(
                    new Uri(URI["photoURI"][i]),
                    $@"D:\\InstagramParse\\{temporaryCounter}.jpg");
                    temporaryCounter += 1;
                    while (wc.IsBusy)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                }
            }
        }
        public static void DownloadVideoToFolder(Dictionary<string, List<string>> URI)
        {
            using (WebClient wc = new WebClient())
            {
                var temporaryCounter = 1;
                for (int i = 0; i < URI["videoURI"].Count; i++)
                {
                    wc.DownloadFileAsync(
                        new Uri(URI["videoURI"][i]),
                        $@"D:\\InstagramParse\\{temporaryCounter}video.mp4");
                    temporaryCounter += 1;
                    while (wc.IsBusy)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                }
            }
        }
    }
}
