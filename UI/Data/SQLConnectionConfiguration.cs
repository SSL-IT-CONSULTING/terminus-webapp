using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace terminus_webapp.Data
{
    public class SQLConnectionConfiguration
    {

        public SQLConnectionConfiguration(string value) => Value = value;
        public string Value { get; }
    }
}
