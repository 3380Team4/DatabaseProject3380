using System;
using System.Collections.Generic;

namespace ThemeParkApplication.Models
{
    public partial class Attractions
    {
        public Attractions()
        {
            Closures = new HashSet<Closures>();
            Employees = new HashSet<Employees>();
            Maintenance = new HashSet<Maintenance>();
            Merchandise = new HashSet<Merchandise>();
        }

        public string AttractionName { get; set; }
        public string AttractionId { get; set; }
        public string ManagerId { get; set; }
        public int AttractionType { get; set; }
        public int? HeightRequirement { get; set; }
        public int? AgeRequirement { get; set; }
        public int AttractionStatus { get; set; }

        public AttractionStatusTable AttractionStatusNavigation { get; set; }
        public AttractionTypeTable AttractionTypeNavigation { get; set; }
        public Employees Manager { get; set; }
        public ICollection<Closures> Closures { get; set; }
        public ICollection<Employees> Employees { get; set; }
        public ICollection<Maintenance> Maintenance { get; set; }
        public ICollection<Merchandise> Merchandise { get; set; }
    }
}
