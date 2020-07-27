using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace terminus.shared.models
{
    public class rptPropertypeInventoryMV
    {

        public Guid id { get; set; }

        [MaxLength(1000)]
        public string description { get; set; }
        [MaxLength(100)]
        public string propertyType { get; set; }
        [MaxLength(100)]
        public string TenantName { get; set; }
        [MaxLength(100)]
        public string contactNumber { get; set; }
        [MaxLength(100)]
        public string emailAddress { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime dateFrom { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime dateTo { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime dueDate { get; set; }



        public decimal areaInSqm { get; set; }

        [MaxLength(50)]
        public string ownerLastName { get; set; }

        [MaxLength(50)]
        public string ownerFirstName { get; set; }

        [MaxLength(50)]
        public string ownerMiddleName { get; set; }

        [MaxLength(100)]
        public string ownerFullName { get; set; }

        [MaxLength(1000)]
        public string ownerAddress { get; set; }

        [MaxLength(100)]
        public string ownerEmailAdd { get; set; }

        [MaxLength(100)]
        public string ownerContactNo { get; set; }

        [MaxLength(100)]
        public string ownerRemarks { get; set; }



        public decimal monthlyRate { get; set; }
        public decimal associationDues { get; set; }

        public decimal penaltyPct { get; set; }

        public decimal ratePerSQM { get; set; }

        public decimal totalBalance { get; set; }

        public bool withWT { get; set; }

        public decimal ratePerSQMAssocDues { get; set; }


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
        public string workAddress { get; set; }

        [MaxLength(100)]
        public string emergy_FullName { get; set; }

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
    } 
}
