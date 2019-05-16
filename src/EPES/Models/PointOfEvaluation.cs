using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPES.Models
{
    public class PointOfEvaluation
    {
        public int Id { get; set; }

        [Display(Name = "ปีงบประมาณ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Year { get; set; }

        [Display(Name = "ลำดับที่")]
        public int Point { get; set; }

        [Display(Name = "ลำดับที่ย่อย")]
        public int SubPoint { get; set; }

        [Display(Name = "แผน")]
        public TypeOfPlan? Plan { get; set; } 

        [Display(Name = "แผนงาน/ตัวชี้วัด")]
        public string Name { get; set; }

        [Display(Name = "หน่วยนับ")]
        public string Unit { get; set; }

        [Display(Name = "น้ำหนักร้อยละ")]
        [Column(TypeName = "decimal(7, 4)")]
        public decimal Weight { get; set; }

        [Display(Name = "เกณฑ์การให้คะแนน 1 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate1 { get; set; }

        [Display(Name = "เกณฑ์การให้คะแนน 2 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate2 { get; set; }

        [Display(Name = "เกณฑ์การให้คะแนน3 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate3 { get; set; }

        [Display(Name = "เกณฑ์การให้คะแนน 4 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate4 { get; set; }

        [Display(Name = "เกณฑ์การให้คะแนน 5 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate5 { get; set; }

        public int? OwnerOfficeId { get; set; }
        [Display(Name = "ตัวชี้วัดสำหรับ")]
        [DisplayFormat(NullDisplayText = "สภ./สท.")]
        public Office OwnerOffice { get; set; }

        public int? AuditOfficeId { get; set; }
        [Display(Name = "หน่วยงานกำกับ")]
        public Office AuditOffice { get; set; }

        [Display(Name = "ข้อมูลในการประเมิน")]
        public ICollection<DataForEvaluation> DataForEvaluations { get; set; }

        public string UpdateUserId { get; set; }
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
