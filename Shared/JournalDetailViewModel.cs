using System;
using System.Collections.Generic;
using System.Text;

namespace terminus.shared.models
{
    public class JournalDetailViewModel
    {

        //Posting Date	Entry Date	DR (Amt)	CR (Amt)	Description	Reference	Remarks

        public DateTime postingDate { get; set; }
        public DateTime transactionDate { get; set; }

        public decimal? drAmt { get; set; }
        public decimal? crAmt { get; set; }

        public string description { get; set; }

        public string reference { get; set; }
        public string remarks { get; set; }

    }
}
