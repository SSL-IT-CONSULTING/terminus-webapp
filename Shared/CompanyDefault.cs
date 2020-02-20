using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace terminus.shared.models
{
    public class CompanyDefault
    {
        [Key]
        [MaxLength(10)]
        public string companyId { get; set; }

        [ForeignKey("RevenueMonthlyDueDebitAccount")]
        public Guid? RevenueMonthlyDueDebitAccountId { get; set; }
        public GLAccount RevenueMonthlyDueDebitAccount { get; set; }

        [ForeignKey("RevenueMonthlyDueAccount")]
        public Guid? RevenueMonthlyDueAccountId { get; set; }
        public GLAccount RevenueMonthlyDueAccount { get; set; }

        [ForeignKey("RevenueMonthlyDueVatAccount")]
        public Guid? RevenueMonthlyDueVatAccountId { get; set; }
        public GLAccount RevenueMonthlyDueVatAccount { get; set; }


        [ForeignKey("RevenueAssocDuesDebitAccount")]
        public Guid? RevenueAssocDuesDebitAccountId { get; set; }
        public GLAccount RevenueAssocDuesDebitAccount { get; set; }

        [ForeignKey("RevenueAssocDuesAccount")]
        public Guid? RevenueAssocDuesAccountId { get; set; }
        public GLAccount RevenueAssocDuesAccount { get; set; }


        [ForeignKey("RevenueAssocDuesVatAccount")]
        public Guid? RevenueAssocDuesVatAccountId { get; set; }
        public GLAccount RevenueAssocDuesVatAccount { get; set; }

    }
}
