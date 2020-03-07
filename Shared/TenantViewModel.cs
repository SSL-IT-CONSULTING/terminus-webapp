using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace terminus.shared.models
{
    public class TenantViewModel:TBase
    {

        [Key, MaxLength(36)]
        public string id { get; set; }

        [MaxLength(20)]
        public string companyid { get; set; }


        public Company company { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string lastName { get; set; }

        [Required]
        [MaxLength(100)]
        public string firstName { get; set; }


        [MaxLength(100)]
        public string middleName { get; set; }

        [Required]
        [MaxLength(20)]
        public string contactNumber { get; set; }

        
        [MaxLength(300)]
        public string emailAddress { get; set; }

        public string propertyDirectoryId { get; set; }

        public string propertyid { get; set; }
        public List<Property> properties { get; set; }


        public DateTime dateFrom { get; set; }
        public DateTime dateTo { get; set; }

        public decimal monthlyRate { get; set; }

        public string revenueAccountId { get; set; }

        public string status { get; set; }

        public decimal associationDues { get; set; }

        public decimal penaltyPct { get; set; }

        [Required]
        public decimal ratePerSQM { get; set; }

        public decimal totalBalance { get; set; }

        public string withWT { get; set; }

        [Required]
        public decimal ratePerSQMAssocDues { get; set; }

        public List<TenantDocument> tenantDocument { get; set; }

    }
}
