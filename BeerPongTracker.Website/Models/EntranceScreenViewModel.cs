using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeerPongTracker.Website.Models
{
    public class EntranceScreenViewModel
    {
        public string Name { get; set; }

        public string FacebookId { get; set; }

        public YouTubeVideoModel YouTubeVideoModel { get; set; }
    }
}