using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace terminus.shared.models
{
    public class rptExpensesVM
    {
        public Guid id { get; set; }
        public DateTime transactionDate { get; set; }

        [MaxLength(100)]
        public string Payee { get; set; }
        [MaxLength(100)]
        public string receiptNo { get; set; }
        [MaxLength(100)]
        public string expenseTitle { get; set; }
        public Decimal amount { get; set; }
        [MaxLength(1000)]
        public string remarks { get; set; }

        

    }
}
