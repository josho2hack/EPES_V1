using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPES.Models
{
    public class End
    {
        public int Id { get; set; }
        public string EndName { get; set; }
        public int ThemeID { get; set; }
        public Theme Theme { get; set; }
        public bool IsActive { get; set; }
    }
}
