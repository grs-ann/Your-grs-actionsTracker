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
using System.Linq;

namespace your_grs_actionsTracker.InstaSharperActions
{
    class Parsing
    {
        private IInstaApi API { get; set; }
        public Parsing(IInstaApi _instaApi)
        {
            this.API = _instaApi;
        }
        // 
        public async Task<List<string>> GetMedia(string parseObject)
        {
            List<string> URIses = new List<string>();
            IResult<InstaMediaList> media = await API.GetUserMediaAsync(parseObject, PaginationParameters.Empty);
            Environment.ExpandEnvironmentVariables(@"D:\InstagramParse");
            foreach (var value in media.Value)
            {
                var images = value.Images;
                if (images.Count > 0)
                {
                    var max = images.Max(i => i.Height);
                    // Filtering by maximum value of image height
                    // The reason for this is that media.Value returns list that
                    // contains view and pre-view image.
                    URIses.Add((from x in images where (x.Height == max) select x).FirstOrDefault().URI);
                }
                if (value.Videos.Count > 0)
                {
                }
            }
            return URIses;
        }
    }
}
