using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPES.Models
{
    public class PointOfEvaluation
    {
        public int Id { get; set; }

        [Display(Name = "ปี")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Year { get; set; }

        [Display(Name = "ตัวชี้วัด")]
        public int Point { get; set; }

        [Display(Name = "ตัวชี้วัดย่อย")]
        public int SubPoint { get; set; }

        [Display(Name = "แผน")]
        public TypeOfPlan? Plan { get; set; } 

        [Display(Name = "ชื่อ/รายละเอียด")]
        public string Name { get; set; }

        [Display(Name = "หน่วยนับ")]
        public string Unit { get; set; }

        [Display(Name = "น้ำหนัก")]
        [Column(TypeName = "decimal(7, 4)")]
        public decimal Weight { get; set; }

        [Display(Name = "ค่าที่ได้ 1 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate1 { get; set; }

        [Display(Name = "ค่าที่ได้ 2 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate2 { get; set; }

        [Display(Name = "ค่าที่ได้ 3 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate3 { get; set; }

        [Display(Name = "ค่าที่ได้ 4 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate4 { get; set; }

        [Display(Name = "ค่าที่ได้ 5 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate5 { get; set; }

        [Display(Name = "หน่วยงานเจ้าของ")]
        public Office OwnerOffice { get; set; }

        [Display(Name = "หน่วยงานตรวจสอบ")]
        public Office AuditOffice { get; set; }

        [Display(Name = "ข้อมูลในการประเมิน")]
        public ICollection<DataForEvaluation> DataForEvaluations { get; set; }

        [Display(Name = "ผู้แก้ไขล่าสุด")]
        public ApplicationUser UpdateUser { get; set; }
    }

    public enum TypeOfPlan
    {
        A,
        B,
        C,
        D
    }
}
