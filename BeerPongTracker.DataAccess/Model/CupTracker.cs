//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BeerPongTracker.DataAccess.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class CupTracker
    {
        public int CupTrackerId { get; set; }
        public int GameId { get; set; }
        public int TeamId { get; set; }
        public int CupId { get; set; }
        public bool Active { get; set; }
    
        public virtual Game Game { get; set; }
    }
}
