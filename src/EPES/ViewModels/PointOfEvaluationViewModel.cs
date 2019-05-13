using EPES.Models;
using System;
using System.Collections.Generic;
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

        public int yearPoint { get; set; }
    }
}
