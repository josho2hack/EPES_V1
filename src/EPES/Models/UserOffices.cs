using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EPES.Models
{
    public class UserOffices
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public int OfficeId { get; set; }
        public Office Office { get; set; }
    }
}
