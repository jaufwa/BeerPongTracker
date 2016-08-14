using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeerPongTracker.Website.Models
{
    public class WinnerPhotosViewModel
    {
        public IEnumerable<WinnerPhotoViewModel> Photos { get; set; }
    }

    public class WinnerPhotoViewModel
    {
        public string FacebookId { get; set; }
    }
}