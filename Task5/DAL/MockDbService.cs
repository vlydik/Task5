using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task5.Models;

namespace Task5.DAL
{
    public class MockDbService : IDbService
    {
        private static IEnumerable<Student> _students;

        static MockDbService()
        {
            _students = new List<Student>
            {
                new Student{IndexNumber = $"s{new Random().Next(1, 2000)}", FirstName= "Jan", LastName= "Kowalski"},

                new Student{IndexNumber = $"s{new Random().Next(1, 2000)}", FirstName = "Anna", LastName = "Malewska"},

                new Student{IndexNumber = $"s{new Random().Next(1, 2000)}", FirstName="Andrzej", LastName="Andrzejewski"},

                new Student{IndexNumber = $"s{new Random().Next(1, 2000)}", FirstName="Michal", LastName="Tomaszewski"}
            };
        }

        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }
    }
}
