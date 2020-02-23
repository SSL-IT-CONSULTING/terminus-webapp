using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace terminus.shared.models
{
    public class Property:TBase
    {
        [Key, MaxLength(36)]
        public string id { get; set; }

        [ForeignKey("company")]
        [MaxLength(10)]
        public string companyId { get; set; }
        public Company company { get; set; }

        [MaxLength(100)]
        public string description { get; set; }

        [MaxLength(1000)]
        public string address { get; set; }

        [MaxLength(20)]
        public string propertyType { get; set; }

        public decimal areaInSqm { get; set; }

    }
}
