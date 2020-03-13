using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using EPES.Models;

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

        public bool hasSub { get; set; }

        public Approve? Approve { get; set; }

        public string Comment1 { get; set; }
        public string Comment2 { get; set; }
        public string Comment3 { get; set; }
        public string Comment4 { get; set; }

        //public IFormFile FileUpload { get; set; }
    }
}
