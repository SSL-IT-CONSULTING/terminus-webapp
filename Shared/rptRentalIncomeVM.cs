using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace terminus.shared.models
{
    public class rptRentalIncomeVM
    {

        public Guid id { get; set; }
        [MaxLength(100)]
        public string description { get; set; }
        [MaxLength(100)]
        public string propertyType { get; set; }
        [MaxLength(100)]
        public string TenantName { get; set; }

        public DateTime transactionDate { get; set; }

        public DateTime dateFrom { get; set; }

        public DateTime dateTo { get; set; }

        public DateTime dueDate { get; set; }

        [MaxLength(100)]
        public string ReceivableTitle { get; set; }
        [MaxLength(100)]
        public string ModeOfPayment { get; set; }
        [MaxLength(100)]
        public string reference { get; set; }
        public decimal amount { get; set; }

        [MaxLength(1000)]
        public string remarks { get; set; }
    }
}
