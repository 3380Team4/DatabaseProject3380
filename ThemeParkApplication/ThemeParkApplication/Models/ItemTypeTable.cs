using System;
using System.Collections.Generic;

namespace ThemeParkApplication.Models
{
    public partial class ItemTypeTable
    {
        public ItemTypeTable()
        {
            Merchandise = new HashSet<Merchandise>();
        }

        public int ItemTypeIndex { get; set; }
        public string ItemType { get; set; }

        public ICollection<Merchandise> Merchandise { get; set; }
    }
}
