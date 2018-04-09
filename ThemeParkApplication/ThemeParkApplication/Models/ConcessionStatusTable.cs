using System;
using System.Collections.Generic;

namespace ThemeParkApplication.Models
{
    public partial class ConcessionStatusTable
    {
        public ConcessionStatusTable()
        {
            Concessions = new HashSet<Concessions>();
        }

        public int ConcessionStatusIndex { get; set; }
        public string ConcessionStatus { get; set; }

        public ICollection<Concessions> Concessions { get; set; }
    }
}
