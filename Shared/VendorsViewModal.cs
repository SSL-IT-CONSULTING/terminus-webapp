using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace terminus.shared.models
{
    public class VendorsViewModal
    {
        [Key, MaxLength(36)]
        public string vendorId { get; set; }

        [MaxLength(200)]
        public string vendorName { get; set; }

        public string companyId { get; set; }
        public List<Company> company { get; set; }

        public string isVatRegistered { get; set; }

        [MaxLength(36)]
        public string inputVatAccountid { get; set; }
        public List<GLAccount> inputVatAccount { get; set; }

        public int rowOrder { get; set; }
    }
}
