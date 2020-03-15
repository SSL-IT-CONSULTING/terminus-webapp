using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace terminus.shared.models
{
    public class PropertyViewModel
    {
        [Key, MaxLength(36)]
        public string id { get; set; }

        [ForeignKey("company")]
        [MaxLength(10)]
        public string companyId { get; set; }
        public Company company { get; set; }

        [Required ]
        [MaxLength(100)]
        public string description { get; set; }

        [Required]
        [MaxLength(1000)]
        public string address { get; set; }

        [Required]
        [MaxLength(20)]
        public string propertyType { get; set; }

        [Required]
        public decimal areaInSqm { get; set; }

        public List<PropertyDocument> propertyDocument { get; set; }


        [MaxLength(50)]
        public string ownerLastName { get; set; }

        [MaxLength(50)]
        public string ownerFirstName { get; set; }

        [MaxLength(50)]
        public string ownerMiddleName { get; set; }

        [MaxLength(100)]
        public string ownerFullName { get; set; }

        [MaxLength(1000)]
        public string ownerAddress { get; set; }

        [MaxLength(100)]
        public string ownerEmailAdd { get; set; }

        [MaxLength(100)]
        public string ownerContactNo { get; set; }

        [MaxLength(100)]
        public string ownerRemarks { get; set; }
    }
}
