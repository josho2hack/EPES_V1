using EPES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPES.ViewModels
{
    public class ResultViewModel
    {
        public IList<PointOfEvaluation> pointA { get; set; }
        public IList<PointOfEvaluation> pointB { get; set; }
        public IList<PointOfEvaluation> pointC { get; set; }
        public IList<PointOfEvaluation> pointD { get; set; }

        public IList<Office> Offices { get; set; }

        public Office Office { get; set; }
        public PointOfEvaluation Point { get; set; }

        public int month { get; set; }
        public int yearPoint { get; set; }
        public int poeid { get; set; }
    }
}
