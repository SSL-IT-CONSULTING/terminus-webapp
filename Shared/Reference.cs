using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace terminus.shared.models
{
    public class Reference : TBase
    {
  
        public Guid Id { get; set; }

        [MaxLength(100)]
        public string ReferenceType { get; set; }

        [MaxLength(200)]
        public string ReferenceCode { get; set; }

        [MaxLength(2000)]
        public string ReferenceValue { get; set; }

        [MaxLength(2000)]
        public string Remarks { get; set; }

        [MaxLength(4)]
        public int SeqNo { get; set; }

        //[MaxLength(1)]
        //public string DeleteSw { get; set; }

        //[MaxLength(40)]
        //public string CreatedBy { get; set; }

        //[MaxLength(12)]
        //public DateTime CreatedDate { get; set; }

        //[MaxLength(40)]
        //public string UpdatedBy { get; set; }

        //[MaxLength(12)]
        //public DateTime UpdateDate { get; set; }

    }
}
