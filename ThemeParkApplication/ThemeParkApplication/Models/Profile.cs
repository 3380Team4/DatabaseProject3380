using System;
using System.Collections.Generic;

namespace ThemeParkApplication.Models
{
    public partial class Profile
    {
        public string EmployeeId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public byte[] Salt { get; set; }
        public bool Status { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime? LastLoggedIn { get; set; }

        public Employees Employee { get; set; }
    }
}
