using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPES.Models
{
    public class Score
    {
        public int Id { get; set; }

        [Display(Name = "คะแนน")]
        public int Value { get; set; }

        [Display(Name = "เดือนล่าสุด")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMMM}", ApplyFormatInEditMode = true)]
        public DateTime LastMonth { get; set; }

        public int PointOfEvaluationID { get; set; }
        [Display(Name = "ตัวชี้วัด")]
        public PointOfEvaluation PointOfEvaluation { get; set; }

        public int OfficeID { get; set; }
        [Display(Name = "หน่วยงาน")]
        public Office Office { get; set; }
    }
}
