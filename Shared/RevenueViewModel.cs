using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace terminus.shared.models
{
    public class RevenueViewModel : TransactionViewModel
    {
        [MaxLength(36)]
        public string id { get; set; }
        public string propertyId { get; set; }

        [MaxLength(100)]
        public string propertyDescription { get; set; }

        [MaxLength(1000)]
        public string propertyAddress { get; set; }

        [MaxLength(36)]
        public string tenantId { get; set; }

        [MaxLength(300)]
        public string tenantName { get; set; }

        [MaxLength(36), Required]
        public string propertyDirectoryId { get; set; }

        public List<GLAccount> revenueAccounts { get; set; }

        public List<PropertyDirectory> propertyDirectories { get; set; }

        public string outputVatAccount { get; set; }
        

    }
}
