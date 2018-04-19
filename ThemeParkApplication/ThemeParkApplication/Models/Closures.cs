using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace ThemeParkApplication.Models
{
    public partial class Closures
    {
        public string ClosureId { get; set; }
        public int Reason { get; set; }
        public string ConcId { get; set; }
        public string AttrId { get; set; }
        public DateTime DateClosure { get; set; }
        public string DurationClosure { get; set; }

        public Attractions Attr { get; set; }
        public Concessions Conc { get; set; }
        public ReasonTable ReasonNavigation { get; set; }
    }
}
