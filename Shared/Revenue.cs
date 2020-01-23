using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace terminus.shared.models
{
    [Table("Revenues")]

    public class Revenue: TransactionBase
    {
        public Guid id { get; set; }

        public PropertyDirectory propertyDirectory { get; set; }

      


       
    }
}
