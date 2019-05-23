using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EPES.Models
{
    public class DataForEvaluation
    {
        public int Id { get; set; }

        [Display(Name = "เป้าหมาย/ประมาณการ")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(38, 10)")]
        public decimal? Expect { get; set; }

        [Display(Name = "ผลการดำเนินการ")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(38, 10)")]
        public decimal? Result { get; set; }

        [Display(Name = "ผลการดำเนินการก่อนผู้ตรวจสอบแก้ไข")]
        [Column(TypeName = "decimal(38, 10)")]
        public decimal? OldResult { get; set; }

        [Display(Name = "เดือน")]
        [Range(1,12)]
        public int Month { get; set; }

        [Display(Name = "หมายเหตุการแก้ไข")]
        public String AuditComment { get; set; }

        [Display(Name = "การอนุมัติ")]
        public Approve Approve { get; set; } = Approve.รอพิจารณา;

        [Display(Name = "หมายเหตุการอนุมัติ")]
        public string CommentApprove { get; set; }

        public int PointOfEvaluationId { get; set; }
        [Display(Name = "ตัวชี้วัด")]
        public PointOfEvaluation PointOfEvaluation { get; set; }

        public int OfficeId { get; set; }
        [Display(Name = "หน่วยงาน")]
        public Office Office { get; set; }

        public string UpdateUserId { get; set; }
        [Display(Name = "ผู้แก้ไขล่าสุด")]
        public ApplicationUser UpdateUser { get; set; }

        //public DataForEvaluation()
        //{
        //    Approve = Approve.รอพิจารณา;
        //}
    }

    public enum Approve
    {               
        รอพิจารณา ,กำลังดำเนินการ, เห็นชอบ
    }
}
