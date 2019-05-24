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

        [Display(Name = "แผนงาน/โครงการ")]
        public string DetailPlan { get; set; }

        [Display(Name = "ตัวชี้วัดผลการปฏิบัติราชการ")]
        public string Name { get; set; }

        [Display(Name = "หน่วยนับ")]
        public UnitOfPoint? Unit { get; set; }

        [Display(Name = "น้ำหนักร้อยละ")]
        [Column(TypeName = "decimal(7, 4)")]
        public decimal Weight { get; set; }

        [Display(Name = "เกณฑ์การให้คะแนน 1 คะแนน")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate1 { get; set; }

        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 1 คะแนน รอบที่ 1")]
        public string DetailRate1 { get; set; }

        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 1 คะแนน รอบที่ 2")]
        public string Detail2Rate1 { get; set; }

        [Display(Name = "เกณฑ์การให้คะแนน 2 คะแนน")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate2 { get; set; }

        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 2 คะแนน รอบที่ 1")]
        public string DetailRate2 { get; set; }
        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 2 คะแนน รอบที่ 2")]
        public string Detail2Rate2 { get; set; }

        [Display(Name = "เกณฑ์การให้คะแนน 3 คะแนน")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate3 { get; set; }

        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 3 คะแนน รอบที่ 1")]
        public string DetailRate3 { get; set; }
        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 3 คะแนน รอบที่ 2")]
        public string Detail2Rate3 { get; set; }

        [Display(Name = "เกณฑ์การให้คะแนน 4 คะแนน")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate4 { get; set; }

        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 4 คะแนน รอบที่ 1")]
        public string DetailRate4 { get; set; }
        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 4 คะแนน รอบที่ 2")]
        public string Detail2Rate4 { get; set; }

        [Display(Name = "เกณฑ์การให้คะแนน 5 คะแนน")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate5 { get; set; }

        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 5 คะแนน รอบที่ 1")]
        public string DetailRate5 { get; set; }
        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 5 คะแนน รอบที่ 2")]
        public string Detail2Rate5 { get; set; }

        [Display(Name = "ตัวชี้วัดสำหรับ")]
        public int? OwnerOfficeId { get; set; }
        [Display(Name = "ตัวชี้วัดสำหรับ")]
        [DisplayFormat(NullDisplayText = "สภ./สท.")]
        public Office OwnerOffice { get; set; }

        [Display(Name = "หน่วยงานกำกับ")]
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

    public enum UnitOfPoint
    {
        ร้อยละ,
        ระดับ
    }
}
