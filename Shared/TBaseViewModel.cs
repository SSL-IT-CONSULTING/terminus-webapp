using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace terminus.shared.models
{
    public class TBaseViewModel
    {
        [MaxLength(20)]
        public string companyId { get; set; }

        [MaxLength(100)]
        public string createdBy { get; set; }



        public DateTime createDate { get; set; }



        [MaxLength(100)]
        public string updatedBy { get; set; }


        public DateTime updateDate { get; set; }

        public bool deleted { get; set; }
    }
}
