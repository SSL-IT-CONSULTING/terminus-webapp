using System;
using System.ComponentModel.DataAnnotations;

namespace terminus.shared.models
{
    public class CheckDetails
    {
        [Key]
        public Guid checkDetailId { get; set; }
        
        [MaxLength(300)]
        public string bankName { get; set; }

        [MaxLength(300)]
        public string branch { get; set; }

        public DateTime checkDate { get; set; }

        public decimal amount { get; set; }


    }
}