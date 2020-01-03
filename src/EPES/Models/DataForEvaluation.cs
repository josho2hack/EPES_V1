using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace EPES.Models
{
    public class DataForEvaluation
    {
        private static IFormatProvider enCulture = CultureInfo.CreateSpecificCulture("en-US");

        [Key]
        public int Id { get; set; }

        [Display(Name = "เป้าหมาย/ประมาณการ")]
        [Column(TypeName = "decimal(38, 10)")]
        public decimal Expect { get; set; }

        [Display(Name = "ผลการดำเนินการ")]
        [Column(TypeName = "decimal(38, 10)")]
        public decimal Result { get; set; }

        [Display(Name = "ผลการดำเนินการก่อนผู้ตรวจสอบแก้ไข")]
        [Column(TypeName = "decimal(38, 10)")]
        public decimal? OldResult { get; set; }

        [Display(Name = "เดือน")]
        [Range(1,12)]
        public int Month { get; set; }

        [Display(Name = "การอนุมัติ")]
        public Approve Approve { get; set; } = Approve.รอพิจารณา;

        [Display(Name = "หมายเหตุการอนุมัติ (หัวหน้าหน่วยงาน)")]
        public string CommentApproveLevel1 { get; set; }

        [Display(Name = "หมายเหตุการอนุมัติ (สภ./ผู้กำกับตัวชี้วัด)")]
        public string CommentApproveLevel2 { get; set; }

        [Display(Name = "หมายเหตุการอนุมัติ (กองผู้กำกับตัวชี้วัด)")]
        public string CommentApproveLevel3 { get; set; }

        [Display(Name = "หมายเหตุการอนุมัติ (ผษ.)")]
        public string CommentApproveLevel4 { get; set; }

        [Display(Name = "วันที่แล้วเสร็จ")]
        [DataType(DataType.Date)]
        public DateTime? CompletedDate { get; set; }

        [NotMapped]
        [DataType(DataType.Date)]
        public string Completed
        {
            get { return this.CompletedDate?.ToString("yyyy-MM-dd", enCulture); }
            set { this.CompletedDate = DateTime.Parse(value, enCulture); }
        }

        [Display(Name = "ไฟล์แนบ")]
        public string AttachFile { get; set; }

        [NotMapped]
        [Display(Name = "ไฟล์ Upload")]
        public IFormFile FileUpload { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public DateTime TimeUpdate
        {
            get
            {
                return this.timeUpdate.HasValue
                   ? this.timeUpdate.Value
                   : DateTime.Now;
            }

            set { this.timeUpdate = value; }
        }

        private DateTime? timeUpdate = null;

        public string UpdateUserId { get; set; }
        [Display(Name = "ผู้แก้ไขล่าสุด")]
        public ApplicationUser UpdateUser { get; set; }

        public int OfficeId { get; set; }
        [Display(Name = "หน่วยงาน")]
        public Office Office { get; set; }

        public int RoundId { get; set; }
        [Display(Name = "รอบการวัด")]
        public Round Round { get; set; }
    }

    public enum Approve
    {               
        รอพิจารณา , หัวหน้าหน่วยงานอนุมัติ, สภ_ผู้กำกับตัวชี้วัดอนุมัติ, กองผู้กำกับตัวชี้วัดอนุมัติ,ผษ_อนุมัติ
    }
}
