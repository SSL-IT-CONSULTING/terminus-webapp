using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace terminus.shared.models
{
    public class ReferenceViewModal
    {


        public Guid id  { get; set; }
        [MaxLength(100)]
        public string ReferenceType { get; set; }

        [MaxLength(200)]
        public string ReferenceCode { get; set; }

        [MaxLength(2000)]
        public string ReferenceValue { get; set; }

        public DateTime AsOfDate { get; set; }
    }
}
