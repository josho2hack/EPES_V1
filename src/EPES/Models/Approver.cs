using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPES.Models
{
    public class Approver
    {
        public int NO { get; set; }
        public string OFFICEID { get; set; }
        public string OFFICENAME { get; set; }
        public ApproverDetail Detail { get; set; }
    }

    public class ApproverDetail
    {
        public string UserID { get; set; }
        public string UserPIN { get; set; }
        public string UserRDCode { get; set; }
        public string UserPrefix { get; set; }
        public string UserNameTH { get; set; }
        public string UserSurNameTH { get; set; }
        public string UserNameEN { get; set; }
        public string UserSurNameEN { get; set; }
        public string UserEMail { get; set; }
        public string UserType { get; set; }
        public string UserRank { get; set; }
        public string UserWorkCode { get; set; }
    }
}
