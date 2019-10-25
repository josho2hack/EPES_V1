using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EPES.Models
{
    public class LevelRate
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "เกณฑ์การให้คะแนน 1 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate1 { get; set; } = 1;

        [Display(Name = "เกณฑ์การให้คะแนน 2 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate2 { get; set; } = 2;

        [Display(Name = "เกณฑ์การให้คะแนน 3 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate3 { get; set; } = 3;

        [Display(Name = "เกณฑ์การให้คะแนน 4 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate4 { get; set; } = 4;

        [Display(Name = "เกณฑ์การให้คะแนน 5 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate5 { get; set; } = 5;

        [Display(Name = "หมายเลขตัวชี้วัด")]
        public int PointOfEvaluationId { get; set; }
        [Display(Name = "ตัวชี้วัด")]
        public PointOfEvaluation PointOfEvaluation { get; set; }
    }
}
