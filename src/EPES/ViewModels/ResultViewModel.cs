using EPES.Models;
using Microsoft.AspNetCore.Http;
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
        public IList<PointOfEvaluation> pointFlagship { get; set; }
        public IList<PointOfEvaluation> pointCascade { get; set; }
        public IList<PointOfEvaluation> pointJointKPI { get; set; }

        //public IList<Office> Offices { get; set; }

        //public Office Office { get; set; }
        //public PointOfEvaluation Point { get; set; }

        public int month { get; set; }
        public int yearPoint { get; set; }
        public string selectoffice { get; set; }
        public int destoffice { get; set; }
        //public int poeid { get; set; }

        //public IFormFile MyProperty { get; set; }
    }
}
