using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace terminus.shared.models
{
    public class TPersonBase:TBase
    {
        [Key, MaxLength(36)]
        public string id { get; set; }

        public Company company { get; set; }

        [MaxLength(100)]
        public string lastName { get; set; }

        [MaxLength(100)]
        public string firstName { get; set; }

        [MaxLength(100)]
        public string middleName { get; set; }

        [MaxLength(20)]
        public string contactNumber { get; set; }

        [MaxLength(300)]
        public string emailAddress { get; set; }




    }
}
