using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace terminus.shared.models
{
    public class ReportParameterViewModel
    {

        [Key, MaxLength(8)]
        public int Id { get; set; }

        public DateTime AsOfDate { get; set; }


        [MaxLength(1000)]
        public string ReportType { get; set; }

        public List<ReferenceViewModal> ReferenceVM { get; set; }
    }
}
