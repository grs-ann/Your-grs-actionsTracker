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
        public async Task<Dictionary<string, List<string>>> GetMedia(string parseObject, Dictionary<string, List<string>> URIContainer)
        {
            
            List<string> PhotoURI = new List<string>();
            List<string> VideoURI = new List<string>();
            IResult<InstaMediaList> media = await API.GetUserMediaAsync(parseObject, PaginationParameters.Empty);
            Environment.ExpandEnvironmentVariables(@"D:\InstagramParse");
            foreach (var value in media.Value)
            {
                var images = value.Images;
                if (value.MediaType != InstaMediaType.Video && images.Count > 0)
                {
                    var max = images.Max(i => i.Height);
                    // Filtering by maximum value of image height
                    // The reason for this is that media.Value returns list that
                    // contains view and pre-view image.
                    URIContainer["photoURI"].Add((from x in images where (x.Height == max) select x).FirstOrDefault().URI);
                }
                var videos = value.Videos;
                if (value.MediaType != InstaMediaType.Image && videos.Count > 0)
                {
                    URIContainer["videoURI"].Add(value.Videos.FirstOrDefault().Url);
                }
            }
            return URIContainer;
        }
    }
}
