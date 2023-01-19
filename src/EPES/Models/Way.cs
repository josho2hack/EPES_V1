using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPES.Models
{
    public class Way
    {
        public int Id { get; set; }
        public string WayName { get; set; }
        public int EndID { get; set; }
        public End End { get; set; }
        public bool IsActive { get; set; }
    }
}
