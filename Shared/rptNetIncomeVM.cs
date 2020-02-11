using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace terminus.shared.models
{
    public class rptNetIncomeVM
    {
        public Guid id { get; set; }

        [MaxLength(100)]
        public string Revenue { get; set; }
        public Nullable<decimal> YearToDate { get; set; }

    }
}
