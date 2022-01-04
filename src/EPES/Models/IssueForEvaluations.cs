using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EPES.Models
{
    public class IssueForEvaluations
    {

        [Key]
        public int Id { get; set; }
        
        [Display(Name = "เดือน")]
        [Range(1, 12)]
        public int Month { get; set; }

        public int PointOfEvaluationId { get; set; }
        [Display(Name = "ตัวชี้วัด")]
        public virtual PointOfEvaluation PointOfEvaluations { get; set; }

        public int OfficeId { get; set; }
        [Display(Name = "หน่วยงาน")]
        public Office Office { get; set; }

        [Display(Name = "ปัญหาและอุปสรรค/แนวทางแก้ไข")]
        public string Issue { get; set; }
    }
}
