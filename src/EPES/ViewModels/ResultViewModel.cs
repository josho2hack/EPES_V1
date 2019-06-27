using EPES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPES.ViewModels
{
    public class ResultViewModel
    {
        public IEnumerable<PointOfEvaluation> pointA { get; set; }
        public IEnumerable<PointOfEvaluation> pointB { get; set; }
        public IEnumerable<PointOfEvaluation> pointC { get; set; }
        public IEnumerable<PointOfEvaluation> pointD { get; set; }

        public IEnumerable<Office> Offices { get; set; }

        public Office Office { get; set; }
        public PointOfEvaluation Point { get; set; }

        public int yearPoint { get; set; }
        public int poeid { get; set; }
    }
}
