using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace terminus.shared.models
{
    public class RevenueViewModel
    {
        [MaxLength(36)]
        public string id { get; set; }

        [MaxLength(36)]
        public string documentId { get; set; }
        public string propertyId { get; set; }

        [MaxLength(100)]
        public string propertyDescription { get; set; }

        [MaxLength(1000)]
        public string propertyAddress { get; set; }

        [MaxLength(36)]
        public string tenantId { get; set; }

        [MaxLength(300)]
        public string tenantName { get; set; }

        [MaxLength(300)]
        public string ownerName { get; set; }

        [MaxLength(36)]
        public string propertyDirectoryId { get; set; }

        public List<GLAccount> revenueAccounts { get; set; }

        public List<Property> properties { get; set; }
        public List<Tenant> tenants { get; set; }
        public string outputVatAccount { get; set; }

        [MaxLength(8)]
        public string billingType { get; set; }

        public bool collect { get; set; }

        public List<RevenueLineItemViewModel> revenueLineItems { get; set; }


        [Required]
        public DateTime transactionDate { get; set; }

        [Display(Name = "Details")]
        [MaxLength(36)]
        public string glAccountId { get; set; }

        [MaxLength(120)]
        public string glAccountCode { get; set; }


        [MaxLength(1000)]
        public string glAccountName { get; set; }


        [MaxLength(200), Required]
        public string description { get; set; }

        [Required]
        [Display(Name = "Due date")]
        public DateTime? dueDate { get; set; }

        [MaxLength(500)]
        public string remarks { get; set; }

        //[Required]
        [Display(Name = "Amount")]
        public decimal amount { get; set; }

        [MaxLength(100), Required]

        [Display(Name = "Reference")]
        public string reference { get; set; }

        public string cashOrCheck { get; set; }

        [MaxLength(300)]
        public string bankName { get; set; }

        [MaxLength(300)]
        public string branch { get; set; }

        public DateTime? checkDate { get; set; }
        public DateTime? checkReleaseDate { get; set; }

        public DateTime? checkDepositDate { get; set; }

        public decimal checkAmount { get; set; }

        [MaxLength(50)]
        public string checkNo { get; set; }

        [Display(Name = "Ledger")]
        [MaxLength(36)]
        public string cashAccountId { get; set; }

        [MaxLength(120)]
        public string cashAccountCode { get; set; }

        [MaxLength(1000)]
        public string cashAccountName { get; set; }


        public JournalEntryHdr journalEntry { get; set; }

        [MaxLength(36), Required]
        public string receiptNo { get; set; }

        [Display(Name = "Billing Id")]
        [MaxLength(36), Required]
        public string billingDocumentId { get; set; }

        [MaxLength(36)]
        public string billingId { get; set; }

        public decimal taxAmount
        {
            get; set;

        }

        public decimal beforeTax { get; set; }

    }
}
