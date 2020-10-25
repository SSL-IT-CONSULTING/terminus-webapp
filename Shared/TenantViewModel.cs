using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace terminus.shared.models
{
    public class TenantViewModel : TBase
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

        [Required]
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


        [MaxLength(100)]
        public string TenantIDs { get; set; }

        [MaxLength(1)]
        public string Owned_Mgd { get; set; }

        [MaxLength(100)]
        public string MgtFeePct { get; set; }


        [MaxLength(100)]
        public string CCTNumber { get; set; }

        [MaxLength(50)]
        public string contactNo2 { get; set; }

        [MaxLength(100)]
        public string contactNo3 { get; set; }

        [MaxLength(1000)]
        public string homeAddress { get; set; }

        [MaxLength(1000)]
        public string workAddress { get; set; }

        [MaxLength(100)]
        public string emergyFullName { get; set; }

        [MaxLength(50)]
        public string emergyContactNo { get; set; }

        [MaxLength(1000)]
        public string emergyAdrress { get; set; }

        [MaxLength(100)]
        public string emergyRelationshipOwner { get; set; }

        public bool otherRestenanted1 { get; set; }

        public bool otherResResiding1 { get; set; }

        [MaxLength(100)]
        public string otherResFullName1 { get; set; }

        [MaxLength(100)]
        public string otherResRelationshipToOwner1 { get; set; }


        public bool otherRestenanted2 { get; set; }

        public bool otherResResiding2 { get; set; }

        [MaxLength(100)]
        public string otherResFullName2 { get; set; }

        [MaxLength(100)]
        public string otherResRelationshipToOwner2 { get; set; }


        public bool otherRestenanted3 { get; set; }

        public bool otherResResiding3 { get; set; }

        [MaxLength(100)]
        public string otherResFullName3 { get; set; }

        [MaxLength(100)]
        public string otherResRelationshipToOwner3 { get; set; }


        [MaxLength(100)]
        public string subTenantFullName1 { get; set; }

        [MaxLength(100)]
        public string subTenantID1 { get; set; }

        [MaxLength(1000)]
        public string subTenantHomeAddress1 { get; set; }

        [MaxLength(1000)]
        public string subTenantWorkAddress1 { get; set; }

        [MaxLength(1000)]
        public string subTenantContactNo1 { get; set; }

        [MaxLength(100)]
        public string subTenantEmailAdd1 { get; set; }

        [MaxLength(100)]
        public string RelToPrimary1 { get; set; }


        [MaxLength(100)]
        public string subTenantFullName2 { get; set; }

        [MaxLength(100)]
        public string subTenantID2 { get; set; }

        [MaxLength(1000)]
        public string subTenantHomeAddress2 { get; set; }

        [MaxLength(1000)]
        public string subTenantWorkAddress2 { get; set; }

        [MaxLength(1000)]
        public string subTenantContactNo2 { get; set; }

        [MaxLength(100)]
        public string subTenantEmailAdd2 { get; set; }

        [MaxLength(100)]
        public string RelToPrimary2 { get; set; }


        [MaxLength(100)]
        public string subTenantFullName3 { get; set; }

        [MaxLength(100)]
        public string subTenantID3 { get; set; }

        [MaxLength(1000)]
        public string subTenantHomeAddress3 { get; set; }

        [MaxLength(1000)]
        public string subTenantWorkAddress3 { get; set; }

        [MaxLength(1000)]
        public string subTenantContactNo3 { get; set; }

        [MaxLength(100)]
        public string subTenantEmailAdd3 { get; set; }

        [MaxLength(100)]
        public string RelToPrimary3 { get; set; }


        [MaxLength(100)]
        public string tenantProxID { get; set; }

        [MaxLength(100)]
        public string subTenantProxID1 { get; set; }

        [MaxLength(100)]
        public string subTenantProxID2 { get; set; }

        [MaxLength(100)]
        public string subTenantProxID3 { get; set; }
    }
}
