using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace terminus.shared.models
{
    public class Customer
    {
        [Required]
        [Key, MaxLength(36)]
        public string id { get; set; }

        [ForeignKey("company")]
        [MaxLength(10)]
        public string companyId { get; set; }

        [MaxLength(50)]
        public string lastName { get; set; }
        
        [MaxLength(50)]
        public string firstName { get; set; }
        
        [MaxLength(1000)]
        public string description { get; set; }

        public bool vatRegistered { get; set; }

        [MaxLength(1000)]
        public string address { get; set; }

        [MaxLength(20)]
        public string contactNo1 { get; set; }

        [MaxLength(20)]
        public string contactNo2 { get; set; }

        [MaxLength(20)]
        public string contactNo3 { get; set; }
    }
}
