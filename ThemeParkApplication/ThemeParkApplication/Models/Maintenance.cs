using System;
using System.Collections.Generic;

namespace ThemeParkApplication.Models
{
    public partial class Maintenance
    {
        public string WorkOrderId { get; set; }
        public int OrderType { get; set; }
        public string ConcId { get; set; }
        public string AttrId { get; set; }
        public string MaintenanceEmployeeId { get; set; }
        public int WorkStatus { get; set; }
        public DateTime WorkStartDate { get; set; }
        public DateTime? WorkFinishDate { get; set; }

        public Attractions Attr { get; set; }
        public Concessions Conc { get; set; }
        public Employees MaintenanceEmployee { get; set; }
        public OrderTypeTable OrderTypeNavigation { get; set; }
        public WorkStatusTable WorkStatusNavigation { get; set; }
    }
}
