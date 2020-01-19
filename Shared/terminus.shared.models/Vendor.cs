using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace terminus.shared.models
{
    public class Vendor
    {
        [Key,MaxLength(36)]
        public string vendorId {get;set;}

        [MaxLength(200)]
        public string vendorName { get; set; }

        [ForeignKey("company")]
        [MaxLength(10)]
        public string companyId { get; set; }

        public Company company { get; set; }

        public int rowOrder { get; set; }


    }
}
