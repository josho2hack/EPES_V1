using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPES.Models
{
    public class Rootobject
    {
        public TaxCollection[] taxCollection { get; set; }
    }

    public class TaxCollection
    {
        public decimal CMCYforcast { get; set; }
        public decimal CMcurrentYear { get; set; }
        public decimal CMdiffCYforcastAmt { get; set; }
        public decimal CMdiffCYforcastPercent { get; set; }
        public decimal CMdiffCYlastYearAmt { get; set; }
        public decimal CMdiffCYlastYearPercent { get; set; }
        public decimal CMlastYear { get; set; }
        public decimal CYcurrentYear { get; set; }
        public decimal CYdiffForcastAmt { get; set; }
        public decimal CYdiffForcastPercent { get; set; }
        public decimal CYdiffLastYearAmt { get; set; }
        public decimal CYdiffLastYearPercent { get; set; }
        public decimal CYforcast { get; set; }
        public decimal CYlastYear { get; set; }
        public string formCode { get; set; }
        public string officeCode { get; set; }
        public string officeShortName { get; set; }
        public string taxType { get; set; }
    }
}
