using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [Range(1,50)]
        [DefaultValue(1)]
        public int Point { get; set; }

        [Display(Name = "ตัวชี้วัดย่อย (ถ้ามี)")]
        public int SubPoint { get; set; }

        [Display(Name = "แผน")]
        public TypeOfPlan Plan { get; set; }

        [Display(Name = "แผนงาน/โครงการ")]
        public string DetailPlan { get; set; }

        [Display(Name = "เป้าประสงค์")]
        public ExpectPlanRD? ExpectPlan { get; set; }

        //[Display(Name = "DDRIVE")]
        public DdriveRD? Ddrive { get; set; }

        [Display(Name = "ตัวชี้วัดผลการปฏิบัติราชการ")]
        public string Name { get; set; }

        [Display(Name = "หน่วยวัด")]
        public UnitOfPoint? Unit { get; set; }

        [Display(Name = "น้ำหนักร้อยละ")]
        [Column(TypeName = "decimal(7, 4)")]
        public decimal Weight { get; set; }

        //[Display(Name = "จำนวนรอบการประเมิน")]
        //[NotMapped]
        //public int CountRound { get { return Rounds.Count; } }

        [Display(Name = "รอบการประเมิน")]
        public List<Round> Rounds { get; set; }

        [Display(Name = "ตัวชี้วัดสำหรับ (ID)")]
        public int OwnerOfficeId { get; set; }
        //[DisplayFormat(NullDisplayText = "สภ./สท.")]
        [Display(Name = "ตัวชี้วัดสำหรับ")]
        public Office OwnerOffice { get; set; }

        [Display(Name = "หน่วยงานกำกับ(ID)")]
        public int AuditOfficeId { get; set; }
        [Display(Name = "หน่วยงานกำกับ")]
        public Office AuditOffice { get; set; }

        public string UpdateUserId { get; set; }
        [Display(Name = "ผู้แก้ไขล่าสุด")]
        public ApplicationUser UpdateUser { get; set; }

        [Display(Name = "คะแนน")]
        public IList<Score> Scores { get; set; }

        [Display(Name = "คะแนนเบื้องต้น")]
        public IList<ScoreDraft> ScoreDrafts { get; set; }

        [Display(Name = "ข้อมูลสำหรับคำนวณคะแนน")]
        public IList<DataForEvaluation> DataForEvaluations { get; set; }

        [Display(Name = "โปรแกรมดึงข้อมูลอัตโนมัติ")]
        public AutoApps AutoApp { get; set; } = AutoApps.ไม่มี;

    }

    public enum AutoApps
    {
       ไม่มี,
       ข้อมูลการจัดเก็บภาษีอากร,
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
