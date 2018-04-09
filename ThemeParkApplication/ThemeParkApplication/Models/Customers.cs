using System;
using System.Collections.Generic;

namespace ThemeParkApplication.Models
{
    public partial class Customers
    {
        public Customers()
        {
            Transactions = new HashSet<Transactions>();
        }

        public string CustomerLastName { get; set; }
        public string CustomerFirstName { get; set; }
        public int CustomerId { get; set; }
        public DateTime? LastVisited { get; set; }
        public string Email { get; set; }
        public int NumberVisits { get; set; }

        public ICollection<Transactions> Transactions { get; set; }
    }
}
