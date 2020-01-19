using System;
using System.Collections.Generic;
using System.Text;

namespace terminus.shared.models
{
    public class PropertyDirectory:TBase
    {
        public Guid id { get; set; }

        public Property property { get; set; }
        public Tenant tenant { get; set; }

        public DateTime dateFrom { get; set; }
        public DateTime dateTo { get; set; }

        public Company company { get; set; }

    }
}
