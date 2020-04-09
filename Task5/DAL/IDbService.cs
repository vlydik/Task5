using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task5.Models;

namespace Task5.DAL
{
    public interface IDbService
    {
        public IEnumerable<Student> GetStudents();
    }
}
