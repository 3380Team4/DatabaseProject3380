using System;
using System.Collections.Generic;

namespace ThemeParkApplication.Models
{
    public partial class WorkStatusTable
    {
        public WorkStatusTable()
        {
            Maintenance = new HashSet<Maintenance>();
        }

        public int WorkStatusIndex { get; set; }
        public string WorkStatus { get; set; }

        public ICollection<Maintenance> Maintenance { get; set; }
    }
}
