using EPES.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace EPES.ViewModels
{
    public class ReportViewModel
    {
        private static IFormatProvider enCulture = CultureInfo.CreateSpecificCulture("en-US");

        public TypeOfPlan Plan { get; set; }

        //[Display(Name = "แผน")]
        public string PlanType
        {
            get
            {
                switch (this.Plan)
                {
                    case TypeOfPlan.A:
                        return "A";
                    case TypeOfPlan.B:
                        return "B";
                    case TypeOfPlan.C:
                        return "C";
                    case TypeOfPlan.D:
                        return "D";
                    default:
                        return "";
                }
            }
        }

        public int Point { get; set; }
        public int SubPoint { get; set; }

        //[Display(Name = "ตัวชี้วัด")]
        public string PointFull
        {
            get
            {
                if (this.SubPoint == 0)
                {
                    return this.Point.ToString() + this.Name;
                }
                else
                {
                    return this.Point + "." + this.SubPoint + this.Name;
                }
            }
        }

        //[Display(Name = "รายละเอียดตัวชี้วัด")]
        public string Name { get; set; }

        public int Month { get; set; }

        //[Display(Name = "ปีงบประมาณ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Year { get; set; }

        //[Display(Name = "คะแนนเบื้องต้น")]
        public decimal ScoreDraft { get; set; }

        public IList<Score> Scores { get; set; }

        public decimal Score 
        { get
            {
                decimal value = 0;
                if (Scores != null)
                {
                    foreach (var item in this.Scores)
                    {
                        if (item.LastMonth == this.Month)
                        {
                            value = item.Value;
                        }
                    }
                }
                return value;
            }
        }
    }
}
