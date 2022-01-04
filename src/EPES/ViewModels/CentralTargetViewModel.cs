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
    public class CentralTargetViewModel
    {
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public string Point { get; set; }
        public string PointName { get; set; }
        public string DetailPlan { get; set; }
        public DdriveRD? Ddrive { get; set; }
        public UnitOfPoint? Unit { get; set; }
        public TypeOfPlan Plan { get; set; }
        public decimal Target { get; set; }
        public decimal Result { get; set; }
        public decimal Expect { get; set; }
        public Boolean HasSub { get; set; }
        
    }
}
