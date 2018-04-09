using System;
using System.Collections.Generic;

namespace ThemeParkApplication.Models
{
    public partial class AttractionStatusTable
    {
        public AttractionStatusTable()
        {
            Attractions = new HashSet<Attractions>();
        }

        public int AttractionStatusIndex { get; set; }
        public string AttractionStatus { get; set; }

        public ICollection<Attractions> Attractions { get; set; }
    }
}
