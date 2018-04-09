using System;
using System.Collections.Generic;

namespace ThemeParkApplication.Models
{
    public partial class OrderTypeTable
    {
        public OrderTypeTable()
        {
            Maintenance = new HashSet<Maintenance>();
        }

        public int OrderTypeIndex { get; set; }
        public string OrderType { get; set; }

        public ICollection<Maintenance> Maintenance { get; set; }
    }
}
