using EPES.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPES.ViewModels
{
    public class PointOfEvaluationViewModel
    {
        public IEnumerable<PointOfEvaluation> pointA { get; set; }
        public IEnumerable<PointOfEvaluation> pointB { get; set; }
        public IEnumerable<PointOfEvaluation> pointC { get; set; }
        public IEnumerable<PointOfEvaluation> pointD { get; set; }

        public IEnumerable<PointOfEvaluation> PointList { get; set; }

        public PointOfEvaluation point{ get; set; }
        public Round Round { get; set; }
        public Round Round2 { get; set; }
        public Round LRound { get; set; }
        public Round LRound2 { get; set; }
        public Round LRRound { get; set; }
        public Round LRRound2 { get; set; }

        [Display(Name = "จำนวนรอบ")]
        [Range(1, 2)]
        public int roundNumber { get; set; }

        public int yearPoint { get; set; }
        public string selectoffice { get; set; }

        public decimal expect1 { get; set; } = 0;
        public decimal expect2 { get; set; } = 0;
        public decimal expect3 { get; set; } = 0;
        public decimal expect4 { get; set; } = 0;
        public decimal expect5 { get; set; } = 0;
        public decimal expect6 { get; set; } = 0;
        public decimal expect7 { get; set; } = 0;
        public decimal expect8 { get; set; } = 0;
        public decimal expect9 { get; set; } = 0;
        public decimal expect10 { get; set; } = 0;
        public decimal expect11 { get; set; } = 0;
        public decimal expect12 { get; set; } = 0;
    }
}
