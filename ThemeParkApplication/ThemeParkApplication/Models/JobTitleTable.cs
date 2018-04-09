using System;
using System.Collections.Generic;

namespace ThemeParkApplication.Models
{
    public partial class JobTitleTable
    {
        public JobTitleTable()
        {
            Employees = new HashSet<Employees>();
        }

        public int JobTitleIndex { get; set; }
        public string JobTitle { get; set; }

        public ICollection<Employees> Employees { get; set; }
    }
}
