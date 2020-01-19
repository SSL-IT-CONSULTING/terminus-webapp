using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace terminus.shared.models
{

    public class JEListViewModel
    {
        [Key]
        public Guid accountId { get; set; }
        public string accountCode { get; set; }
        public string accountDesc { get; set; }

        public int rowOrder { get; set; }
        public decimal balance { get; set; }
    }
}
