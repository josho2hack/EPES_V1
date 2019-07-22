using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPES.Models
{
    public class PointOfEvaluation
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "ปีงบประมาณ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Year { get; set; }

        [Display(Name = "ตัวชี้วัดที่")]
        public int Point { get; set; }

        [Display(Name = "ลำดับที่ย่อย")]
        public int SubPoint { get; set; }

        [Display(Name = "แผน")]
        public TypeOfPlan Plan { get; set; }

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
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate1 { get; set; } = 1;

        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 1 คะแนน รอบที่ 1")]
        public string DetailRate1 { get; set; }

        [Display(Name = "เดือนเริ่ม 1 คะแนน รอบที่ 1")]
        public DateTime? Rate1MonthStart { get; set; }
        [Display(Name = "เดือนสิ้นสุด 1 คะแนน รอบที่ 1")]
        public DateTime? Rate1MonthStop { get; set; }

        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 1 คะแนน รอบที่ 2")]
        public string Detail2Rate1 { get; set; }
        [Display(Name = "เดือนเริ่ม 1 คะแนน รอบที่ 2")]
        public DateTime? Rate1MonthStart2 { get; set; }
        [Display(Name = "เดือนสิ้นสุด 1 คะแนน รอบที่ 2")]
        public DateTime? Rate1MonthStop2 { get; set; }

        [Display(Name = "เกณฑ์การให้คะแนน 2 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate2 { get; set; } = 2;

        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 2 คะแนน รอบที่ 1")]
        public string DetailRate2 { get; set; }
        [Display(Name = "เดือนเริ่ม 2 คะแนน รอบที่ 1")]
        public DateTime? Rate2MonthStart { get; set; }
        [Display(Name = "เดือนสิ้นสุด 2 คะแนน รอบที่ 1")]
        public DateTime? Rate2MonthStop { get; set; }

        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 2 คะแนน รอบที่ 2")]
        public string Detail2Rate2 { get; set; }
        [Display(Name = "เดือนเริ่ม 2 คะแนน รอบที่ 2")]
        public DateTime? Rate2MonthStart2 { get; set; }
        [Display(Name = "เดือนสิ้นสุด 2 คะแนน รอบที่ 2")]
        public DateTime? Rate2MonthStop2 { get; set; }

        [Display(Name = "เกณฑ์การให้คะแนน 3 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate3 { get; set; } = 3;

        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 3 คะแนน รอบที่ 1")]
        public string DetailRate3 { get; set; }
        [Display(Name = "เดือนเริ่ม 3 คะแนน รอบที่ 1")]
        public DateTime? Rate3MonthStart { get; set; }
        [Display(Name = "เดือนสิ้นสุด 3 คะแนน รอบที่ 1")]
        public DateTime? Rate3MonthStop { get; set; }
        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 3 คะแนน รอบที่ 2")]
        public string Detail2Rate3 { get; set; }
        [Display(Name = "เดือนเริ่ม 3 คะแนน รอบที่ 2")]
        public DateTime? Rate3MonthStart2 { get; set; }
        [Display(Name = "เดือนสิ้นสุด 3 คะแนน รอบที่ 2")]
        public DateTime? Rate3MonthStop2 { get; set; }

        [Display(Name = "เกณฑ์การให้คะแนน 4 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate4 { get; set; } = 4;

        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 4 คะแนน รอบที่ 1")]
        public string DetailRate4 { get; set; }
        [Display(Name = "เดือนเริ่ม 4 คะแนน รอบที่ 1")]
        public DateTime? Rate4MonthStart { get; set; }
        [Display(Name = "เดือนสิ้นสุด 4 คะแนน รอบที่ 1")]
        public DateTime? Rate4MonthStop { get; set; }
        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 4 คะแนน รอบที่ 2")]
        public string Detail2Rate4 { get; set; }
        [Display(Name = "เดือนเริ่ม 4 คะแนน รอบที่ 2")]
        public DateTime? Rate4MonthStart2 { get; set; }
        [Display(Name = "เดือนสิ้นสุด 4 คะแนน รอบที่ 2")]
        public DateTime? Rate4MonthStop2 { get; set; }

        [Display(Name = "เกณฑ์การให้คะแนน 5 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate5 { get; set; } = 5;

        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 5 คะแนน รอบที่ 1")]
        public string DetailRate5 { get; set; }
        [Display(Name = "เดือนเริ่ม 5 คะแนน รอบที่ 1")]
        public DateTime? Rate5MonthStart { get; set; }
        [Display(Name = "เดือนสิ้นสุด 5 คะแนน รอบที่ 1")]
        public DateTime? Rate5MonthStop { get; set; }
        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 5 คะแนน รอบที่ 2")]
        public string Detail2Rate5 { get; set; }
        [Display(Name = "เดือนเริ่ม 5 คะแนน รอบที่ 2")]
        public DateTime? Rate5MonthStart2 { get; set; }
        [Display(Name = "เดือนสิ้นสุด 5 คะแนน รอบที่ 2")]
        public DateTime? Rate5MonthStop2 { get; set; }

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
        public IList<DataForEvaluation> DataForEvaluations { get; set; }

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
