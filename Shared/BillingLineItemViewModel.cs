using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace terminus.shared.models
{
    public class BillingLineItemViewModel
    {
        public string Id { get; set; }

        [MaxLength(200)]
        public string description { get; set; }

        public decimal amount { get; set; }


    }
}
