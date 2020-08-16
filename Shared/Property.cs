using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace terminus.shared.models
{
    public class Property : TBase
    {
        [Key, MaxLength(36)]
        public string id { get; set; }

        [ForeignKey("company")]
        [MaxLength(10)]
        public string companyId { get; set; }
        public Company company { get; set; }

        [MaxLength(100)]
        public string description { get; set; }

        [MaxLength(1000)]
        public string address { get; set; }

        [MaxLength(20)]
        public string propertyType { get; set; }

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



        [MaxLength(1)]
        public string Owned_Mgd { get; set; }

        [MaxLength(100)]
        public string MgtFeePct { get; set; }


        [MaxLength(100)]
        public string CCTNumber { get; set; }


        [MaxLength(100)]
        public string emergyFullName { get; set; }

        [MaxLength(50)]
        public string emergyContactNo { get; set; }

        [MaxLength(1000)]
        public string emergyAdrress { get; set; }

        [MaxLength(100)]
        public string emergyRelationshipOwner { get; set; }



        public string otherRestenanted { get; set; }

        public string otherResResiding { get; set; }

        [MaxLength(100)]
        public string otherResFullName1 { get; set; }


        [MaxLength(50)]
        public string otherResContactNo1 { get; set; }

        [MaxLength(100)]
        public string otherResRelationshipToOwner1 { get; set; }

        [MaxLength(100)]
        public string otherResFullName2 { get; set; }


        [MaxLength(50)]
        public string otherResContactNo2 { get; set; }

        [MaxLength(100)]
        public string otherResRelationshipToOwner2 { get; set; }


        [MaxLength(100)]
        public string otherResFullName3 { get; set; }


        [MaxLength(50)]
        public string otherResContactNo3 { get; set; }

        [MaxLength(100)]
        public string otherResRelationshipToOwner3 { get; set; }

        [MaxLength(100)]
        public string BuildingCode { get; set; }

    }
}
