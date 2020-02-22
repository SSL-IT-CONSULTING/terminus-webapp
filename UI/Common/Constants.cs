using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace terminus_webapp.Common
{
    public class Constants
    {
        public class BillLineTypes
        {
           
            public const string MONTHLYBILLITEM = "MONTHLYBILLITEM";
            public const string MONTHLYBILLITEM_PREVBAL = "MONTHLYBILLITEM_PREVBAL";
            public const string MONTHLYBILLITEM_VAT = "MONTHLYBILLITEM_VAT";
            public const string MONTHLYBILLITEM_WT = "MONTHLYBILLITEM_WT";
            public const string MONTHLYBILLITEMPENALTY = "MONTHLYBILLITEMPENALTY";

            public const string MONTHLYASSOCDUE = "MONTHLYASSOCDUE";
            public const string MONTHLYASSOCDUE_PREVBAL = "MONTHLYASSOCDUE_PREVBAL";
            public const string MONTHLYASSOCDUEPENALTY = "MONTHLYASSOCDUEPENALTY";
            public const string MONTHLYASSOCDUE_VAT = "MONTHLYASSOCDUE_VAT";

        }
    }
}
