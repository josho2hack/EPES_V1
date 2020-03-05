using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPES.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Display(Name = "คำนำหน้าชื่อ")]
        public string Title { get; set; }

        [PersonalData]
        [Display(Name = "ชื่อ")]
        public string FName { get; set; }

        [PersonalData]
        [Display(Name = "นามสกุล")]
        public string LName { get; set; }

        [PersonalData]
        [Display(Name = "ตำแหน่ง")]
        public string PosName { get; set; }

        [PersonalData]
        [Display(Name = "รหัสหน่วยงาน")]
        public string OfficeId { get; set; }

        [PersonalData]
        [Display(Name = "ชื่อหน่วยงาน")]
        public string OfficeName { get; set; }

        [PersonalData]
        [Display(Name = "หมายเลขบัตรประชาชน")]
        public string PIN { get; set; }

        [PersonalData]
        [Display(Name = "ระดับ")]
        public string Class { get; set; }

        [PersonalData]
        [Display(Name = "ฝ่าย/กลุ่ม")]
        public string GroupName { get; set; }

        [PersonalData]
        [Display(Name = "วันเดือนปีเกิด")]
        public DateTime DOB { get; set; }

        [PersonalData]
        [Display(Name = "เป็นผู้อนุมัติ")]
        public bool? approver { get; set; }

        [Display(Name = "ข้อมูลที่ใช้ในการประเมิน")]
        public ICollection<DataForEvaluation> DataForEvaluations { get; set; }

        //[Display(Name = "ตัวชี้วัด")]
        //public ICollection<PointOfEvaluation> PointOfEvaluations { get; set; }
    }
}
