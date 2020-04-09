using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Task5.Models;
using System.Globalization;
using Task5.DAL;

namespace Task4.Controllers

{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        string[] validFormats = new[] { "MM/dd/yyyy", "yyyy/MM/dd", "MM/dd/yyyy HH:mm:ss", "MM/dd/yyyy hh:mm tt", "yyyy-MM-dd HH:mm:ss, fff" };
        CultureInfo provider = new CultureInfo("en-US");
        private readonly IDbService _dbService;
        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetStudents(string orderBy)
        {
            var students = new List<Student>();
            using (var client = new SqlConnection("Data Source = db-mssql;Initial Catalog = s19183; Integrated Security = True"))
            using (var com = new SqlCommand())
            {
                com.Connection = client;
                com.CommandText = "select * from Student";
                client.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = DateTime.ParseExact(dr["BirthDate"].ToString(), validFormats, provider);
                    st.enrollment = new Enrollment
                    {
                        IdSemester = (int)dr["Semester"],
                        studies = new Studies { study = dr["Name"].ToString() }
                    };
                    students.Add(st);

                }
            }
            return Ok(students);
        }
        [HttpGet("{id}")]
        public IActionResult GetStudent(string id)
        {
            var enrollment = new Enrollment();
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19183;Integrated Security=True"))
            {
                using (var com = new SqlCommand())
                {
                    com.Connection = con;
                    com.CommandText = "select * from Student";
                    com.Parameters.AddWithValue("id", id);
                    con.Open();
                    var dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        enrollment.IdSemester = (int)dr["Semester"];
                    }

                }
            }
            return Ok(enrollment);

        }
        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            var s = new Student();
            s.IndexNumber = $"s{new Random().Next(1, 2000)}";
            s.FirstName = "Jan";
            s.LastName = "Kowalski";
            return Ok(s);
        }
        [HttpPut("{i}")]
        public IActionResult PutStudent(int i)
        {
            var s = new Student();
            s.IndexNumber = $"s{new Random().Next(1, 2000)}";
            s.FirstName = "Jan";
            s.LastName = "Kowalski";
            return Ok(s);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int i)
        {
            return Ok(i);
        }
    }
}