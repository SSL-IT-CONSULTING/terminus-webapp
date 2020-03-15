using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace terminus.shared.models
{
    [Table("Companies")]
    public class Company
    {

       [MaxLength(10), Required]
        [Key]
        public string companyId { get; set; }

        [MaxLength(100), Required]
        public string companyName { get; set; }


        [MaxLength(1000)]
        public string address { get; set; }

    }

}