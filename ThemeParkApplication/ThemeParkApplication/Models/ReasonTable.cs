using System;
using System.Collections.Generic;

namespace ThemeParkApplication.Models
{
    public partial class ReasonTable
    {
        public ReasonTable()
        {
            Closures = new HashSet<Closures>();
        }

        public int ReasonIndex { get; set; }
        public string Reason { get; set; }

        public ICollection<Closures> Closures { get; set; }
    }
}
