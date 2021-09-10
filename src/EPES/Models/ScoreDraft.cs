using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace EPES.Models
{
    public class ScoreDraft
    {
        private static IFormatProvider enCulture = CultureInfo.CreateSpecificCulture("en-US");

        [Key]
        public int Id { get; set; }

        [Display(Name = "คะแนน")]
        [Column(TypeName = "decimal(5, 4)")]
        public decimal ScoreValue { get; set; }

        [Display(Name = "เดือนล่าสุด")]
        [Range(1, 12)]
        public int LastMonth { get; set; }

        public int PointOfEvaluationId { get; set; }
        [Display(Name = "ตัวชี้วัด")]
        public virtual PointOfEvaluation PointOfEvaluation { get; set; }

        public int OfficeId { get; set; }
        [Display(Name = "หน่วยงาน")]
        public Office Office { get; set; }

        [Display(Name = "คะแนนที่อนุมัติ")]
        [Column(TypeName = "decimal(5, 4)")]
        public decimal ScoreApprove { get; set; } = 0;

        [NotMapped]
        [Display(Name = "น้ำหนักร้อยละ")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "decimal(7, 4)")]
        public decimal? weightOfMonth { get; set; } = 0;

        //[NotMapped]
        //[Display(Name = "คะแนนที่อนุมัติ")]
        //[Column(TypeName = "decimal(5, 4)")]
        //public decimal? ScoreApproveToReport
        //{
        //    get
        //    {
        //        if (this.ScoreApprove == 0)
        //        {
        //            return null;
        //        }
        //        else
        //        {
        //            return this.ScoreApprove;
        //        }
        //    }
        //}
    }
}
