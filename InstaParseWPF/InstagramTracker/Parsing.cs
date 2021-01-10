using InstagramApiSharp;
using InstagramApiSharp.API;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace InstaParseWPF.InstagramTracker
{
    class Parsing
    {
        private IInstaApi API { get; set; }
        public Parsing(IInstaApi _instaApi)
        {
            this.API = _instaApi;
        }
        public async Task<Dictionary<string, List<string>>> GetMedia(string parseObject, Dictionary<string, List<string>> URIContainer)
        {
            List<string> PhotoURI = new List<string>();
            List<string> VideoURI = new List<string>();
            IResult<InstaMediaList> media = await API.UserProcessor.GetUserMediaAsync(parseObject, PaginationParameters.Empty);
            foreach (var value in media.Value)
            {
                var images = value.Images;
                var videos = value.Videos;
                if (images.Count > 0 && value.MediaType != InstaMediaType.Video)
                {
                    var max = images.Max(i => i.Height);
                    // Filtering by maximum value of image height
                    // The reason for this is that media.Value returns list that
                    // contains view and pre-view image.
                    URIContainer["photoURI"].Add((from x in images where (x.Height == max) select x).FirstOrDefault().Uri);
                }
                if (videos.Count > 0)
                {
                    URIContainer["videoURI"].Add(value.Videos.FirstOrDefault().Uri);
                }
                if (videos.Count == 0 && images.Count == 0 && value.Carousel.Count > 0)
                {
                    var carousel = value.Carousel;
                    foreach (var carouselItem in carousel)
                    {
                        if (carouselItem.MediaType == InstaMediaType.Image)
                        {
                            var carouselImages = carouselItem.Images;
                            var max = carouselImages.Max(x => x.Height);
                            URIContainer["photoURI"].Add((from x in carouselImages where (x.Height == max) select x).FirstOrDefault().Uri);
                        }
                        else if (carouselItem.MediaType == InstaMediaType.Video)
                        {
                            URIContainer["videoURI"].Add(carouselItem.Videos.FirstOrDefault().Uri);
                        }
                    }
                }
            }
            return URIContainer;
        }
        public async Task<IResult<InstaUserShortList>>GetUserFollowers(string userAddress)
        {
            var usersShortList = await API.UserProcessor.GetUserFollowersAsync(userAddress, PaginationParameters.Empty);
            return usersShortList;
        }


        // For getting a story media.
        public async Task<Dictionary<string, List<string>>> GetStories(string userName, Dictionary<string, List<string>> urises)
        {
            
            if (API != null)
            {
                var user = await API.UserProcessor.GetUserAsync(userName);
                var userId = user.Value.Pk;
                var storyHighlights = await API.StoryProcessor.GetHighlightFeedsAsync(userId);
                foreach (var item in storyHighlights.Value.Items)
                {
                    var storyBlock = await API.StoryProcessor.GetHighlightMediasAsync(item.HighlightId);
                    foreach (var story in storyBlock.Value.Items)
                    {
                        var images = story.ImageList;
                        var videos = story.VideoList;
                        if ((InstaMediaType)story.MediaType == InstaMediaType.Image)
                        {
                            var max = images.Max(i => i.Height);
                            urises["images"].Add((from x in images where (x.Height == max) select x).FirstOrDefault().Uri);
                        }
                        else
                        {
                            urises["videos"].Add(videos.FirstOrDefault().Uri);
                        }
                    }
                }
            }
            return urises;
        }
        
    }
}
