using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace terminus.shared.models
{
    public class Tenant:TPersonBase
    {

        [MaxLength(100)]
        public string TenantIDs { get; set; }

        [MaxLength(50)]
        public string contactNo2 { get; set; }

        [MaxLength(100)]
        public string contactNo3 { get; set; }

        [MaxLength(1000)]
        public string workAddress { get; set; }

        [MaxLength(1000)]
        public string homeAddress { get; set; }

        [MaxLength(100)]
        public string emergyFullName { get; set; }

        [MaxLength(50)]
        public string emergyContactNo { get; set; }

        [MaxLength(1000)]
        public string emergyAdrress { get; set; }

        [MaxLength(100)]
        public string emergyRelationshipOwner { get; set; }

  




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
