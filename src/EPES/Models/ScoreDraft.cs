using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EPES.Models
{
    public class ScoreDraft
    {
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
        public PointOfEvaluation PointOfEvaluation { get; set; }

        public int OfficeId { get; set; }
        [Display(Name = "หน่วยงาน")]
        public Office Office { get; set; }
    }
}
