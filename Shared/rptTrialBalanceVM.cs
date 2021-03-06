﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace terminus.shared.models
{
    public class rptTrialBalanceVM
    {
        [Key, MaxLength(36)]
        public Guid id { get; set; }

        [MaxLength(36)]
        public string accountDesc { get; set; }

        [MaxLength(200)]
        public decimal debit { get; set; }

        [MaxLength(200)]
        public decimal credit { get; set; }
        [MaxLength(200)]
        public decimal amount { get; set; }
    }
}
