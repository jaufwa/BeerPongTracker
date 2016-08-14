using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeerPongTracker.Website.Models
{
    public class WinnerScreenViewModel
    {
        public WinnerNamePlateViewModel NamePlateViewModel { get; set; }

        public WinnerPhotosViewModel WinnerPhotosViewModel { get; set; }
    }
}