using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPES.Models
{
    public class Office
    {
        public int Id { get; set; }

        [Display(Name = "รหัสหน่วยงาน")]
        //[Column(TypeName = "nvarchar(8)")]
        [StringLength(8,ErrorMessage ="รหัสหน่วยงานมี 8 หลัก")]
        public string Code { get; set; }

        [Display(Name = "หน่วยงาน")]
        public string Name { get; set; }

        [Display(Name = "หมายเหตุ")]
        public string Remark { get; set; }

        [Display(Name = "ตัวชี้วัดของหน่วยงาน")]
        [InverseProperty("OwnerOffice")]
        public ICollection<PointOfEvaluation> OwnerPointOfEvaluations { get; set; }

        [Display(Name = "ตัวชี้วัดที่ตรวจสอบ")]
        [InverseProperty("AuditOffice")]
        public ICollection<PointOfEvaluation> AuditPointOfEvaluations { get; set; }

        [Display(Name = "ข้อมูลที่ใช้ในการประเมิน")]
        public ICollection<DataForEvaluation> DataForEvaluations { get; set; }

        [Display(Name = "คะแนนของหน่วยงาน")]
        public ICollection<Score> Scores { get; set; }

        [Display(Name = "เจ้าหน้าที่")]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
