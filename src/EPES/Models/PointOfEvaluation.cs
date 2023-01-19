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

        [Display(Name = "มีตัวชี้วัดย่อย")]
        public bool HasSub { get; set; }

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
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "decimal(7, 4)")]
        public decimal Weight { get; set; }

        [Display(Name = "ใช้น้ำหนักร้อยละนี้ทุกเดือน")]
        public bool WeightAll { get; set; }

        [Display(Name = "คะแนนเริ่มต้นที่ 0")]
        public bool StartZero { get; set; }

        [Display(Name = "รอบการประเมิน")]
        public List<Round> Rounds { get; set; }

        [Display(Name = "หน่วยปฏิบัติ")]
        public int OwnerOfficeId { get; set; }
        //[DisplayFormat(NullDisplayText = "สภ./สท.")]
        [Display(Name = "หน่วยปฏิบัติ")]
        public Office OwnerOffice { get; set; }

        [Display(Name = "หน่วยงานกำกับ")]
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

        [Display(Name = "คงค่าประมาณการเมื่อหมดรอบประเมิน")]
        public bool FixExpect { get; set; }

        [Display(Name = "คำนวณคะแนน ผล/ประมาณการ รายเดือน")]
        public bool CalPerMonth { get; set; }

        [NotMapped]
        [Display(Name = "คงค่าประมาณการเมื่อหมดรอบประเมิน")]
        public bool FixExpectProxy
        {
            get
            {
                return this.FixExpect == true;
            }
            set
            {
                this.FixExpect = value;
            }
        }

        [NotMapped]
        [Display(Name = "คำนวณคะแนน ผล/ประมาณการ รายเดือน")]
        public bool CalPerMonthProxy
        {
            get
            {
                return this.CalPerMonth == true;
            }
            set
            {
                this.CalPerMonth = value;
            }
        }

        [NotMapped]
        public decimal? Target { get; set; } = 0;

        [NotMapped]
        public decimal? ScoreTarget { get; set; } = 0;

        [NotMapped]
        public string Issue { get; set; }

        [Display(Name = "ปัญหาและอุปสรรค/แนวทางแก้ไข")]
        public IList<IssueForEvaluations> IssueForEvaluations { get; set; }
        

        [Display(Name = "เอกสารแนบคำอธิบายตัวชี้วัด")]
        public string AttachFile { get; set; }

        [Display(Name = "ทิศทางการบริหารงาน (Theme)")]
        public int? ThemeId { get; set; }
        [Display(Name = "ทิศทางการบริหารงาน (Theme)")]
        public Theme Theme { get; set; }
        [Display(Name = "เป้าหมาย (END)")]
        public int? EndId { get; set; }
        [Display(Name = "เป้าหมาย (END)")]
        public End End { get; set; }
        [Display(Name = "กลยุทธ์ (Way)")]
        public int? WayId { get; set; }
        [Display(Name = "กลยุทธ์ (Way)")]
        public Way Way { get; set; }

        [NotMapped]
        public bool isSelectedToCopy { get; set; }
    }

    public enum AutoApps
    {
       ไม่มี,
        [Display(Name = "1. การจัดเก็บภาษีอากร")]
        การจัดเก็บภาษีอากร,
        [Display(Name = "2. ผู้เสียภาษีรายใหม่")]
        ผู้เสียภาษีรายใหม่,
        [Display(Name = "3. ผู้เสียภาษีรายใหม่ที่ชำระภาษี")]
        ผู้เสียภาษีรายใหม่ที่ชำระภาษี,
        [Display(Name = "(ยกเลิก) การนำข้อมูลสำรวจไปใช้งาน")]
        การนำข้อมูลสำรวจไปใช้งาน,
        [Display(Name = "5. ร้อยละของการบริหารการคืนภาษี")]
        ร้อยละของการบริหารการคืนภาษี,
        [Display(Name = "6. ผู้ประกอบการรายใหญ่ในท้องที่")]
        ผู้ประกอบการรายใหญ่ในท้องที่,
        [Display(Name = "7. จำนวนแบบที่ยื่นผ่านอินเทอร์เน็ต_ยกเว้น_90_91_94")]
        จำนวนแบบที่ยื่นผ่านอินเทอร์เน็ต_ยกเว้น_90_91_94,
        [Display(Name = "8.จำนวนแบบ_1_30_50_ที่ยื่นผ่านอินเทอร์เน็ต")]
        จำนวนแบบ_1_30_50_ที่ยื่นผ่านอินเทอร์เน็ต,
        [Display(Name = "9. เร่งรัดหนี้")]
        เร่งรัดหนี้,
        [Display(Name = "10. จำหน่ายหนี้")]
        จำหน่ายหนี้,
        [Display(Name = "(ยกเลิก) ใบแจ้งภาษีอากรบนระบบ_DMS")]
        ใบแจ้งภาษีอากรบนระบบ_DMS,
        [Display(Name = "(ยกเลิก) ร้อยละของการตรวจคืนภาษี")]
        ร้อยละของการตรวจคืนภาษี,
        [Display(Name = "13. ร้อยละของการแนะนำและตรวจสอบภาษี")]
        ร้อยละของการแนะนำและตรวจสอบภาษี,
        [Display(Name = "14. ร้อยละของผู้ประกอบการที่ดำเนินการแนะนำ")]
        ร้อยละของผู้ประกอบการที่ดำเนินการแนะนำ,
        [Display(Name = "15. ร้อยละของการดำเนินการงานค้างสอบยันใบกำกับภาษี")]
        ร้อยละของการดำเนินการงานค้างสอบยันใบกำกับภาษี,
        [Display(Name = "16. ร้อยละของการสอบยันใบกำกับภาษีที่ได้รับ")]
        ร้อยละของการสอบยันใบกำกับภาษีที่ได้รับ,
        [Display(Name = "17. ร้อยละของจำนวนรายที่มีผลการสอบยันใบกำกับภาษีพบประเด็นความผิด")]
        ร้อยละของจำนวนรายที่มีผลการสอบยันใบกำกับภาษีพบประเด็นความผิด,
        [Display(Name = "18. ร้อยละของการแนะนำและตรวจสอบภาษีอากรผู้เสียภาษีอากรรายกลางและรายย่อม")]
        ร้อยละของการแนะนำและตรวจสอบภาษีอากรผู้เสียภาษีอากรรายกลางและรายย่อม,
        [Display(Name = "19. ร้อยละของผู้เสียภาษีอากรที่ดำเนินการแนะนำและตรวจสอบภาษีอากรแล้วเสร็จ")]
        ร้อยละของผู้เสียภาษีอากรที่ดำเนินการแนะนำและตรวจสอบภาษีอากรแล้วเสร็จ,
        [Display(Name = "20. การเบิกจ่ายงบประมาณ")]
        การเบิกจ่ายงบประมาณ,
        [Display(Name = "21. งานค้างหนังสือร้องเรียนแหล่งภาษี")]
        งานค้างหนังสือร้องเรียนแหล่งภาษี
    }

    public enum TypeOfPlan
    {
        A,
        B,
        C,
        D,
        Flagship,
        Cascade,
        [Display(Name = "Joint KPI")]
        Joint_KPI
    }

    public enum UnitOfPoint
    {
        ร้อยละ,
        ระดับ,
        [Display(Name = "ระดับ/ร้อยละ")]
        ระดับ_ร้อยละ,
        [Display(Name = "ค่าคะแนน")]
        ค่าคะแนน
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
