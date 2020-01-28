using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace terminus.shared.models
{
    public class TrialBalances
    {

        [Key, MaxLength(36)]
        public string accountDesc { get; set; }

        [MaxLength(200)]
        public decimal amount { get; set; }


    }
}
