using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EPES.ViewModels
{
    public class UpdateDataViewModel
    {
        public int? Id { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "decimal(38, 10)")]
        public decimal Result { get; set; }
        public decimal ResultLevelRate { get; set; }

        public string Completed { get; set; }

        public int poeid { get; set; }
        public int officeid { get; set; }

        //public IFormFile FileUpload { get; set; }
    }
}
