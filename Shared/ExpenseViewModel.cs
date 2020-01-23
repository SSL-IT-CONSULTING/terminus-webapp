using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace terminus.shared.models
{
    public class ExpenseViewModel : TransactionViewModel
    {
        [MaxLength(36)]
        public string id { get; set; }
        [MaxLength(36), Required]
        public string vendorId { get; set; }

        [MaxLength(200)]
        public string vendorName { get; set; }

        [MaxLength(200)]
        public string vendorOther { get; set; }

        public List<GLAccount> expenseAccounts { get; set; }

        public List<Vendor> vendors { get; set; }

    }
}
