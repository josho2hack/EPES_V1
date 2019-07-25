using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPES.ViewModels
{
    public class UpdateDataViewModel
    {
        public int? Id { get; set; }
        public decimal? Result { get; set; }

        public string Completed { get; set; }

        public int poeid { get; set; }
        public int officeid { get; set; }

        //public IFormFile FileUpload { get; set; }
    }
}
