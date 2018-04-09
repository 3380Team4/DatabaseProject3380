using System;
using System.Collections.Generic;

namespace ThemeParkApplication.Models
{
    public partial class Concessions
    {
        public Concessions()
        {
            Closures = new HashSet<Closures>();
            Employees = new HashSet<Employees>();
            Maintenance = new HashSet<Maintenance>();
            Merchandise = new HashSet<Merchandise>();
        }

        public string ConcessionName { get; set; }
        public string ConcessionId { get; set; }
        public string ManagerId { get; set; }
        public int StoreType { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public int ConcessionStatus { get; set; }

        public ConcessionStatusTable ConcessionStatusNavigation { get; set; }
        public Employees Manager { get; set; }
        public StoreTypeTable StoreTypeNavigation { get; set; }
        public ICollection<Closures> Closures { get; set; }
        public ICollection<Employees> Employees { get; set; }
        public ICollection<Maintenance> Maintenance { get; set; }
        public ICollection<Merchandise> Merchandise { get; set; }
    }
}
