using System;
using System.Collections.Generic;

namespace ThemeParkApplication.Models
{
    public partial class Employees
    {
        public Employees()
        {
            Attractions = new HashSet<Attractions>();
            Concessions = new HashSet<Concessions>();
            InverseSupervisor = new HashSet<Employees>();
            Maintenance = new HashSet<Maintenance>();
            Profile = new HashSet<Profile>();
            Transactions = new HashSet<Transactions>();
        }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string EmployeeId { get; set; }
        public string Gender { get; set; }
        public DateTime DateBirth { get; set; }
        public int JobTitle { get; set; }
        public string SupervisorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string WorksAtConc { get; set; }
        public string WorksAtAttr { get; set; }

        public JobTitleTable JobTitleNavigation { get; set; }
        public Employees Supervisor { get; set; }
        public Attractions WorksAtAttrNavigation { get; set; }
        public Concessions WorksAtConcNavigation { get; set; }
        public ICollection<Attractions> Attractions { get; set; }
        public ICollection<Concessions> Concessions { get; set; }
        public ICollection<Employees> InverseSupervisor { get; set; }
        public ICollection<Maintenance> Maintenance { get; set; }
        public ICollection<Profile> Profile { get; set; }
        public ICollection<Transactions> Transactions { get; set; }
    }
}
