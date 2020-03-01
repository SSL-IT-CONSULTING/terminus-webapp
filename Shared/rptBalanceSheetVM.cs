using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace terminus.shared.models
{
    public class rptBalanceSheetVM
    {

        public Guid id { get; set; }

        [MaxLength(100)]
        public string accountDesc { get; set; }
        public decimal amount { get; set; }
    }
}
