using System;
using System.Collections.Generic;

namespace ThemeParkApplication.Models
{
    public partial class Merchandise
    {
        public Merchandise()
        {
            Transactions = new HashSet<Transactions>();
        }

        public string ItemName { get; set; }
        public string ItemId { get; set; }
        public decimal Price { get; set; }
        public int ItemType { get; set; }
        public string ConcId { get; set; }
        public string AttrId { get; set; }

        public Attractions Attr { get; set; }
        public Concessions Conc { get; set; }
        public ItemTypeTable ItemTypeNavigation { get; set; }
        public ICollection<Transactions> Transactions { get; set; }
    }
}
