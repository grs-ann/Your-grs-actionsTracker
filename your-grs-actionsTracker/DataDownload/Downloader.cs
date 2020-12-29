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
        public static void DownloadMediaToFolder(List<string> URIsesList)
        {
            using (WebClient wc = new WebClient())
            {
                Console.WriteLine(URIsesList.Count);
                var temporaryCounter = 1;
                for (int i = 0; i < URIsesList.Count; i++)
                {
                    wc.DownloadFileAsync(
                    new Uri(URIsesList[i]),
                    $@"D:\\InstagramParse\\{temporaryCounter}.jpg");
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
