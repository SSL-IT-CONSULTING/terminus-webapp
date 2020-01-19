using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace terminus.shared.models
{
    public class TBase
    {

        [MaxLength(100)]
        public string createdBy { get; set; }



        public DateTime createDate { get; set; }



        [MaxLength(100)]
        public string updatedBy { get; set; }


        public DateTime? updateDate { get; set; }

        public bool deleted { get; set; }
    }
}
