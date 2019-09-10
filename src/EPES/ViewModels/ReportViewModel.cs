using EPES.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EPES.ViewModels
{
    public class ReportViewModel
    {
        public IList<PointOfEvaluation> p { get; set; }
        //public DataForEvaluation de { get; set; }

        public int month { get; set; }
        public int yearPoint { get; set; }
        public int poeid { get; set; }

        public int indexList { get; set; }
    }
}
