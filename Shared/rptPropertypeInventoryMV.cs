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

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime dateFrom { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime dateTo { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime dueDate { get; set; }
    } 
}
