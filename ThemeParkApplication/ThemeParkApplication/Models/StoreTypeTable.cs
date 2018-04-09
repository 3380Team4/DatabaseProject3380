using System;
using System.Collections.Generic;

namespace ThemeParkApplication.Models
{
    public partial class StoreTypeTable
    {
        public StoreTypeTable()
        {
            Concessions = new HashSet<Concessions>();
        }

        public int StoreTypeIndex { get; set; }
        public string StoreType { get; set; }

        public ICollection<Concessions> Concessions { get; set; }
    }
}
