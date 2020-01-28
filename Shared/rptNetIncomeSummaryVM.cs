using System;
using System.ComponentModel.DataAnnotations;
namespace terminus.shared.models
{
    public class rptNetIncomeSummaryVM
    {

        public Guid id { get; set; }

        [MaxLength(100)]
        public string rentalRevenue { get; set; }
        public decimal January { get; set; }
        public decimal Febuary { get; set; }
        public decimal March { get; set; }
        public decimal April { get; set; }
        public decimal May { get; set; }
        public decimal June { get; set; }
        public decimal July { get; set; }
        public decimal August { get; set; }
        public decimal September { get; set; }
        public decimal October { get; set; }
        public decimal November { get; set; }
        public decimal December { get; set; }
        public decimal Total { get; set; }
    }
}
