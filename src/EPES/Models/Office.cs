using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using EPES.Data;
using EPES.Models;
using System.Linq;

namespace EPES.Models
{
    public class Office
    {

        //private readonly ApplicationDbContext _context;

        //public Office(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        [Key]
        public int Id { get; set; }

        [Display(Name = "รหัสหน่วยงาน")]
        //[Column(TypeName = "nvarchar(8)")]
        [StringLength(8,ErrorMessage ="รหัสหน่วยงานมี 8 หลัก")]
        public string Code { get; set; }

        [Display(Name = "หน่วยงาน")]
        [DisplayFormat(NullDisplayText = "สภ./สท.")]
        public string Name { get; set; }

        [Display(Name = "หมายเหตุ")]
        public string Remark { get; set; }
        
        //public int? OfficeGroupId { get; set; }
        [Display(Name = "สังกัดหน่วยงาน")]
        public Office OfficeGroup { get; set; }

        [NotMapped]
        [Display(Name = "ชื่อสังกัดหน่วยงาน")]
        public string NameGroup
        {
            get
            {
                return "หน่วยงานในสังกัด " + this.Name;
            }
        }

        [Display(Name = "ตัวชี้วัดของหน่วยงาน")]
        //[InverseProperty("OwnerOffice")]
        public IList<PointOfEvaluation> OwnerPointOfEvaluations { get; set; }

        [Display(Name = "ตัวชี้วัดที่ตรวจสอบ")]
        //[InverseProperty("AuditOffice")]
        public IList<PointOfEvaluation> AuditPointOfEvaluations { get; set; }

        [Display(Name = "ข้อมูลที่ใช้ในการประเมิน")]
        public IList<DataForEvaluation> DataForEvaluations { get; set; }

        [Display(Name = "คะแนนของหน่วยงาน")]
        public IList<Score> Scores { get; set; }

        [Display(Name = "เจ้าหน้าที่")]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
