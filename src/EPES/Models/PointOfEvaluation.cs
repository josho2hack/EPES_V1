using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace EPES.Models
{
    public class PointOfEvaluation
    {
        private static IFormatProvider enCulture = CultureInfo.CreateSpecificCulture("en-US");

        [Key]
        public int Id { get; set; }

        [Display(Name = "ปีงบประมาณ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Year { get; set; }

        [Display(Name = "ตัวชี้วัดที่")]
        public int Point { get; set; }

        [Display(Name = "ตัวชี้วัดย่อย (ถ้ามี 0 = ตัวชี้วัดหลัก)")]
        public int SubPoint { get; set; }

        [Display(Name = "แผน")]
        public TypeOfPlan Plan { get; set; }

        [Display(Name = "แผนงาน/โครงการ")]
        public string DetailPlan { get; set; }

        [Display(Name = "เป้าประสงค์")]
        public ExpectPlanRD? ExpectPlan { get; set; }

        [Display(Name = "DDRIVE")]
        public DdriveRD? Ddrive { get; set; }

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
        [DataType(DataType.Date)]
        [Display(Name = "เดือนเริ่ม 1 คะแนน รอบที่ 1")]
        public DateTime? Rate1MonthStart { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R1MStart {
            get { return this.Rate1MonthStart?.ToString("yyyy-MM-dd",enCulture); }
            set { if(value != null) this.Rate1MonthStart = DateTime.Parse(value,enCulture); } }

        [DataType(DataType.Date)]
        [Display(Name = "เดือนสิ้นสุด 1 คะแนน รอบที่ 1")]
        public DateTime? Rate1MonthStop { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R1MStop
        {
            get { return this.Rate1MonthStop?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate1MonthStop = DateTime.Parse(value,enCulture); }
        }

        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 1 คะแนน รอบที่ 2")]
        public string Detail2Rate1 { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนเริ่ม 1 คะแนน รอบที่ 2")]
        public DateTime? Rate1MonthStart2 { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R1MStart2
        {
            get { return this.Rate1MonthStart2?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate1MonthStart2 = DateTime.Parse(value, enCulture); }
        }

        [DataType(DataType.Date)]
        [Display(Name = "เดือนสิ้นสุด 1 คะแนน รอบที่ 2")]
        public DateTime? Rate1MonthStop2 { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R1MStop2
        {
            get { return this.Rate1MonthStop2?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate1MonthStop2 = DateTime.Parse(value, enCulture); }
        }
        [Display(Name = "เกณฑ์การให้คะแนน 2 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate2 { get; set; } = 2;

        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 2 คะแนน รอบที่ 1")]
        public string DetailRate2 { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนเริ่ม 2 คะแนน รอบที่ 1")]
        public DateTime? Rate2MonthStart { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R2MStart
        {
            get { return this.Rate2MonthStart?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate2MonthStart = DateTime.Parse(value, enCulture); }
        }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนสิ้นสุด 2 คะแนน รอบที่ 1")]
        public DateTime? Rate2MonthStop { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R2MStop
        {
            get { return this.Rate2MonthStop?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate2MonthStop = DateTime.Parse(value, enCulture); }
        }
        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 2 คะแนน รอบที่ 2")]
        public string Detail2Rate2 { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนเริ่ม 2 คะแนน รอบที่ 2")]
        public DateTime? Rate2MonthStart2 { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R2MStart2
        {
            get { return this.Rate2MonthStart2?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate2MonthStart2 = DateTime.Parse(value, enCulture); }
        }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนสิ้นสุด 2 คะแนน รอบที่ 2")]
        public DateTime? Rate2MonthStop2 { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R2MStop2
        {
            get { return this.Rate2MonthStop2?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate2MonthStop2 = DateTime.Parse(value, enCulture); }
        }
        [Display(Name = "เกณฑ์การให้คะแนน 3 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate3 { get; set; } = 3;

        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 3 คะแนน รอบที่ 1")]
        public string DetailRate3 { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนเริ่ม 3 คะแนน รอบที่ 1")]
        public DateTime? Rate3MonthStart { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R3MStart
        {
            get { return this.Rate3MonthStart?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate3MonthStart = DateTime.Parse(value, enCulture); }
        }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนสิ้นสุด 3 คะแนน รอบที่ 1")]
        public DateTime? Rate3MonthStop { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R3MStop
        {
            get { return this.Rate3MonthStop?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate3MonthStop = DateTime.Parse(value, enCulture); }
        }
        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 3 คะแนน รอบที่ 2")]
        public string Detail2Rate3 { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนเริ่ม 3 คะแนน รอบที่ 2")]
        public DateTime? Rate3MonthStart2 { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R3MStart2
        {
            get { return this.Rate3MonthStart2?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate3MonthStart2 = DateTime.Parse(value, enCulture); }
        }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนสิ้นสุด 3 คะแนน รอบที่ 2")]
        public DateTime? Rate3MonthStop2 { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R3MStop2
        {
            get { return this.Rate3MonthStop2?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate3MonthStop2 = DateTime.Parse(value, enCulture); }
        }
        [Display(Name = "เกณฑ์การให้คะแนน 4 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate4 { get; set; } = 4;

        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 4 คะแนน รอบที่ 1")]
        public string DetailRate4 { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนเริ่ม 4 คะแนน รอบที่ 1")]
        public DateTime? Rate4MonthStart { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R4MStart
        {
            get { return this.Rate4MonthStart?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate4MonthStart = DateTime.Parse(value, enCulture); }
        }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนสิ้นสุด 4 คะแนน รอบที่ 1")]
        public DateTime? Rate4MonthStop { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R4MStop
        {
            get { return this.Rate4MonthStop?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate4MonthStop = DateTime.Parse(value, enCulture); }
        }
        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 4 คะแนน รอบที่ 2")]
        public string Detail2Rate4 { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนเริ่ม 4 คะแนน รอบที่ 2")]
        public DateTime? Rate4MonthStart2 { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R4MStart2
        {
            get { return this.Rate4MonthStart2?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate4MonthStart2 = DateTime.Parse(value, enCulture); }
        }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนสิ้นสุด 4 คะแนน รอบที่ 2")]
        public DateTime? Rate4MonthStop2 { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R4MStop2
        {
            get { return this.Rate4MonthStop2?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate4MonthStop2 = DateTime.Parse(value, enCulture); }
        }
        [Display(Name = "เกณฑ์การให้คะแนน 5 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate5 { get; set; } = 5;

        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 5 คะแนน รอบที่ 1")]
        public string DetailRate5 { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนเริ่ม 5 คะแนน รอบที่ 1")]
        public DateTime? Rate5MonthStart { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R5MStart
        {
            get { return this.Rate5MonthStart?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate5MonthStart = DateTime.Parse(value, enCulture); }
        }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนสิ้นสุด 5 คะแนน รอบที่ 1")]
        public DateTime? Rate5MonthStop { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R5MStop
        {
            get { return this.Rate5MonthStop?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate5MonthStop = DateTime.Parse(value, enCulture); }
        }
        [Display(Name = "รายละเอียดเกณฑ์การให้คะแนน 5 คะแนน รอบที่ 2")]
        public string Detail2Rate5 { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "เดือนเริ่ม 5 คะแนน รอบที่ 2")]
        public DateTime? Rate5MonthStart2 { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R5MStart2
        {
            get { return this.Rate5MonthStart2?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate5MonthStart2 = DateTime.Parse(value, enCulture); }
        }
        [DataType(DataType.Date)]
        [Display(Name = "เดือนสิ้นสุด 5 คะแนน รอบที่ 2")]
        public DateTime? Rate5MonthStop2 { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public string R5MStop2
        {
            get { return this.Rate5MonthStop2?.ToString("yyyy-MM-dd", enCulture); }
            set { if (value != null) this.Rate5MonthStop2 = DateTime.Parse(value, enCulture); }
        }

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

        [Display(Name = "คะแนน")]
        public IList<Score> Scores { get; set; }

        [Display(Name = "คะแนนเบื้องต้น")]
        public IList<ScoreDraft> ScoreDrafts { get; set; }

        [Display(Name = "เกณฑ์การให้คะแนน 4.2 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal LRate1 { get; set; } = 1;

        [Display(Name = "เกณฑ์การให้คะแนน 4.4 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal LRate2 { get; set; } = 2;

        [Display(Name = "เกณฑ์การให้คะแนน 4.6 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal LRate3 { get; set; } = 3;

        [Display(Name = "เกณฑ์การให้คะแนน 4.8 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal LRate4 { get; set; } = 4;

        [Display(Name = "เกณฑ์การให้คะแนน 5.0 คะแนน")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal LRate5 { get; set; } = 5;
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
        ระดับ,
        [Display(Name = "ระดับ/ร้อยละ")]
        ระดับ_ร้อยละ
    }

    public enum ExpectPlanRD
    {
        [Display(Name = "O1 กำหนดนโยบายภาษีให้มีความเหมาะสม เพื่อพัฒนาประเทศที่ยั่งยืน")]
        O1,
        [Display(Name = "O2 เพิ่มประสิทธิภาพการจัดเก็บภาษี")]
        O2,
        [Display(Name = "O3 บูรณาการข้อมูลรายได้ รายจ่าย")]
        O3,
        [Display(Name = "O4 พัฒนาการแนะนำ กำกับดูแล ตรวจสอบภาษีอากรที่มีประสิทธิภาพ")]
        O4,
        [Display(Name = "O5 การเสริมสร้างการปฏิบัติหน้าที่ทางภาษีอากรให้ถูกต้อง")]
        O5,
        [Display(Name = "O6 ปรับปรุงระเบียบ/กฎหมาย/แนวปฏิบัติครบคลุมและเข้าใจง่าย")]
        O6,
        [Display(Name = "O7 พัฒนาบริการที่ดี มุ่งสู่ประชาชน")]
        O7,
        [Display(Name = "O8 พัฒนาบุคลากร: เก่ง ดี มีความสุข")]
        O8,
        [Display(Name = "O9 พัฒนาองค์กรให้เป็น Smart Office")]
        O9,
        [Display(Name = "O10 ส่งเสริม Innovation Culture")]
        O10,
        [Display(Name = "O11 สื่อสารองค์กรเชิงรุก")]
        O11
    }

    public enum DdriveRD
    {
        [Display(Name = "D - Digital Transformation")]
        D1,
        [Display(Name = "D - Data Analytics")]
        D2,
        [Display(Name = "R - Revenue Collection")]
        R,
        [Display(Name = "I – Innovation")]
        I,
        [Display(Name = "V – Values")]
        V,
        [Display(Name = "E – Efficiency")]
        E
    }
}
