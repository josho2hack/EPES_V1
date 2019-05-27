using EPES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPES.ViewModels
{
    public class DataForEvaluationViewModel
    {
        public IEnumerable<PointOfEvaluation> pointA { get; set; }
        public IEnumerable<PointOfEvaluation> pointB { get; set; }
        public IEnumerable<PointOfEvaluation> pointC { get; set; }
        public IEnumerable<PointOfEvaluation> pointD { get; set; }
        //public IEnumerable<DataForEvaluation> dataP { get; set; }
        public IEnumerable<Office> Offices { get; set; }

        public PointOfEvaluation Point { get; set; }

        public int yearPoint { get; set; }
        public int poeid { get; set; }
        public decimal? expect1 { get; set; }
        public decimal? expect2 { get; set; }
        public decimal? expect3 { get; set; }
        public decimal? expect4 { get; set; }
        public decimal? expect5 { get; set; }
        public decimal? expect6 { get; set; }
        public decimal? expect7 { get; set; }
        public decimal? expect8 { get; set; }
        public decimal? expect9 { get; set; }
        public decimal? expect10 { get; set; }
        public decimal? expect11 { get; set; }
        public decimal? expect12 { get; set; }
    }
}
