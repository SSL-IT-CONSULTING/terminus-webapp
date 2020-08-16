using System;
using System.Collections.Generic;
using System.Text;

namespace terminus.shared.models
{
    public class rptCollectionVM
    {

        public Guid id { get; set; }

        public string unit { get; set; }
        public string tenantName { get; set; }
        public string classifications { get; set; }
        public decimal gross { get; set; }
        public decimal rentalRate { get; set; }
        public decimal dues { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public decimal January_Rent { get; set; }
        public decimal January_Due { get; set; }
        public decimal Febuary_Rent { get; set; }
        public decimal Febuary_Due { get; set; }
        public decimal March_Rent { get; set; }
        public decimal March_Due { get; set; }
        public decimal April_Rent { get; set; }
        public decimal April_Due { get; set; }
        public decimal May_Rent { get; set; }
        public decimal May_Due { get; set; }
        public decimal June_Rent { get; set; }
        public decimal June_Due { get; set; }
        public decimal July_Rent { get; set; }
        public decimal July_Due { get; set; }
        public decimal August_Rent { get; set; }
        public decimal August_Due { get; set; }
        public decimal September_Rent { get; set; }
        public decimal September_Due { get; set; }
        public decimal October_Rent { get; set; }
        public decimal October_Due { get; set; }
        public decimal November_Rent { get; set; }
        public decimal November_Due { get; set; }
        public decimal December_Rent { get; set; }
        public decimal December_Due { get; set; }

    }
}
