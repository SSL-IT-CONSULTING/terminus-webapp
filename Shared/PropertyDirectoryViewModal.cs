using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace terminus.shared.models
{
    public class PropertyDirectoryViewModal:TBase

    {


        public Guid id { get; set; }

            
        [MaxLength(36)]
        public string propertyId { get; set; }
        
        public Property propertyEntity { get; set; }

        public List<Property> property { get; set; }

        [MaxLength(500)]
        public string propertyDesc { get; set; }

        [MaxLength(36)]            
        public string tenandId { get; set; }
        public List<Tenant> tenant { get; set; }

        [MaxLength(50)]
        public string tenantLastNName { get; set; }
        [MaxLength(50)]
        public string tenantFirtsName { get; set; }

        public DateTime dateFrom { get; set; }
        public DateTime dateTo { get; set; }

            
        [MaxLength(10)]
        public string companyId { get; set; }
        public Company company { get; set; }

        public decimal monthlyRate { get; set; }
            
        public Guid? revenueAccountId { get; set; }

        public List<GLAccount> revenueAccount { get; set; }

        [MaxLength(12)]
        public string status { get; set; }

        public decimal associationDues { get; set; }

        public decimal penaltyPct { get; set; }

        public decimal ratePerSQM { get; set; }

        public decimal totalBalance { get; set; }

        public string withWT { get; set; }

    }
}

