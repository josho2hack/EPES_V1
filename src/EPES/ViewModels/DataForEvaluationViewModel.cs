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
        public Office Office { get; set; }

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

        public decimal? result1 { get; set; }
        public decimal? result2 { get; set; }
        public decimal? result3 { get; set; }
        public decimal? result4 { get; set; }
        public decimal? result5 { get; set; }
        public decimal? result6 { get; set; }
        public decimal? result7 { get; set; }
        public decimal? result8 { get; set; }
        public decimal? result9 { get; set; }
        public decimal? result10 { get; set; }
        public decimal? result11 { get; set; }
        public decimal? result12 { get; set; }
    }
}
