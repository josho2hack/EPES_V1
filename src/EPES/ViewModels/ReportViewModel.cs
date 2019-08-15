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

        public int month { get; set; }

        //[Display(Name = "คะแนนเบื้องต้น")]
        //[Column(TypeName = "decimal(5, 4)")]
        //public decimal ScoreDraft { get; set; }

        //[Display(Name = "คะแนน")]
        //[Column(TypeName = "decimal(5, 4)")]
        //public decimal Score { get; set; }

        //[Display(Name = "เดือนล่าสุด")]
        //[Range(1, 12)]
        //public int LastMonth { get; set; }
    }
}
