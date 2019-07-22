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

        [DataType(DataType.Date)]
        public DateTime? CompletedDate { get; set; }

        public int poeid { get; set; }
        public int officeid { get; set; }

        //public int month { get; set; }
    }
}
