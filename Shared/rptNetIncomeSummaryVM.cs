using System;
using System.ComponentModel.DataAnnotations;
namespace terminus.shared.models
{
    public class rptNetIncomeSummaryVM
    {

        public Guid id { get; set; }

        [MaxLength(100)]
        public string rentalRevenue { get; set; }
        public Nullable<decimal> January { get; set; }
        public Nullable<decimal> Febuary { get; set; }
        public Nullable<decimal> March { get; set; }
        public Nullable<decimal> April { get; set; }
        public Nullable<decimal> May { get; set; }
        public Nullable<decimal> June { get; set; }
        public Nullable<decimal> July { get; set; }
        public Nullable<decimal> August { get; set; }
        public Nullable<decimal> September { get; set; }
        public Nullable<decimal> October { get; set; }
        public Nullable<decimal> November { get; set; }
        public Nullable<decimal> December { get; set; }
        public Nullable<decimal> Total { get; set; }
    }
}
