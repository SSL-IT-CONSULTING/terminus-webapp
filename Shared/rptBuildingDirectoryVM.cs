using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace terminus.shared.models
{
    public class rptBuildingDirectoryVM
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
        public DateTime dueDate { get; set; }



        [MaxLength(100)]
        public string otherResResiding { get; set; }

        [MaxLength(100)]
        public string otherRestenanted { get; set; }
        [MaxLength(100)]
        public string ownerProxID{ get; set; }
        [MaxLength(100)]
        public string ownerFullName{ get; set; }
        [MaxLength(100)]
        public string otherResProxID1{ get; set; }
		[MaxLength(100)]
        public string otherResFullName1{ get; set; }
		
        [MaxLength(100)]
        public string otherResProxID2{ get; set; }
		
        [MaxLength(100)]
        public string otherResFullName2{ get; set; }
		
        [MaxLength(100)]
        public string otherResProxID3{ get; set; }
		
        [MaxLength(100)]
        public string otherResFullName3{ get; set; }
	
        [MaxLength(100)]
        public string tenantProxID { get; set; }
        
        [MaxLength(100)]

        public string subTenantProxID1{ get; set; }
		
        [MaxLength(100)]
        public string subTenantFullName1{ get; set; }
		
        [MaxLength(100)]
        public string subTenantProxID2{ get; set; }
		
        [MaxLength(100)]
        public string subTenantFullName2{ get; set; }
		
        [MaxLength(100)]
        public string subTenantProxID3{ get; set; }
		
        [MaxLength(100)]
        public string subTenantFullName3{ get; set; }
    }
}
