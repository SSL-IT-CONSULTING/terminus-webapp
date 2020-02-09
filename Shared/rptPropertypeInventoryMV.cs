using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace terminus.shared.models
{
    public class rptPropertypeInventoryMV
    {

        public Guid id { get; set; }

        [MaxLength(1000)]
        public string description { get; set; }
        [MaxLength(100)]
        public string propertyType { get; set; }
        [MaxLength(100)]
        public string TenantName { get; set; }
        [MaxLength(100)]
        public string contactNumber { get; set; }
        [MaxLength(100)]
        public string emailAddress { get; set; }
        public Nullable<DateTime> dateFrom { get; set; }
        public Nullable<DateTime> dateTo { get; set; }
        public Nullable<DateTime> dueDate { get; set; }
    } 
}
