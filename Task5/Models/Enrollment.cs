using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task5.Models
{
    public class Enrollment
    {
        public int IdSemester { get; set; }
        public string StartDate { get; set; }
        public Studies studies { get; set; }
    }
}
