using System;
using System.Collections.Generic;

namespace ThemeParkApplication.Models
{
    public partial class AttractionTypeTable
    {
        public AttractionTypeTable()
        {
            Attractions = new HashSet<Attractions>();
        }

        public int AttractionTypeIndex { get; set; }
        public string AttractionType { get; set; }

        public ICollection<Attractions> Attractions { get; set; }
    }
}
