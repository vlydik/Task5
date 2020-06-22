using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task5.Models;
using Task5.DTOs.Requests;
using System.Data.SqlClient;
using Task5.DTOs.Responses;

namespace Task5.Controllers
{
    [Route("api/enrollments")]
    [ApiController] //implicit model validation
    public class EnrollmentsController : ControllerBase
    {
        [HttpPost(Name = "EnrollStudent")]
        public IActionResult EnrollStudent(EnrollStudentRequests request)
        {

            var response = new EnrollStudentResponse();
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19183;Integrated Security=True"))
            {
                using (var com = new SqlCommand())
                {
                    com.Connection = con;
                    com.CommandText = "SELECT * FROM Studies WHERE Name = @Name";
                    com.Parameters.AddWithValue("Name", request.studies);
                    con.Open();
                    var tran = con.BeginTransaction();
                    com.Transaction = tran;
                    var dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        dr.Close();
                        tran.Rollback();
                        return BadRequest("Wrong studies, try again.");
                    }
                    var idStudy = (int)dr["IdStudy"];
                    dr.Close();
                    com.CommandText = "SELECT * FROM Enrollment WHERE Semester = 1 AND IdStudy = @idStudy";
                    var IdEnrollment = (int)dr["IdEnrollment"] + 1;
                    com.Parameters.AddWithValue("IdStudy", idStudy);
                    dr = com.ExecuteReader();
                    if (dr.Read())
                    {
                        dr.Close();
                        com.CommandText = "SELECT MAX(idEnrollment) AS 'idEnrollment' FROM Enrollment";
                        dr = com.ExecuteReader();
                        dr.Close();
                        DateTime StartDate = DateTime.Now;
                        com.CommandText = "INSERT INTO Enrollment(IdEnrollment, Semester, IdStudy, StartDate) VALUES (@IdEnrollemnt, 1, @IdStudy, @StartDate)";
                        com.Parameters.AddWithValue("IdEnrollment", IdEnrollment);
                        com.Parameters.AddWithValue("StartDate", StartDate);
                        com.ExecuteNonQuery();
                    }
                    dr.Close();
                    com.CommandText = "SELECT * FROM Student WHERE IndexNumber=@IndexNumber";
                    com.Parameters.AddWithValue("IndexNumber", request.IndexNumber);
                    dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        dr.Close();
                        com.CommandText = "INSERT INTO Student(IndexNumber, FirstName, LastName, Birthdate, IdEnrollment) VALUES (@IndexNumber, @FirstName, @LastName, @BirthDate, @IdEnrollment)";
                        com.Parameters.AddWithValue("FirstName", request.FirstName);
                        com.Parameters.AddWithValue("LastName", request.LastName);
                        com.Parameters.AddWithValue("BirthDate", request.BirthDate);
                        com.Parameters.AddWithValue("IdEnrollment", IdEnrollment);
                        com.ExecuteNonQuery();
                        dr.Close();
                    }
                    else
                    {
                        dr.Close();
                        tran.Rollback();
                        return BadRequest("Error");
                    }
                    tran.Commit();
                }



                return Ok("Success");
            }
        }
        [HttpPost(Name = "PromoteStudent")]
        public IActionResult PromoteStudent(PromoteStudentResponse response)
        {
            PromoteStudentResponse result = new PromoteStudentResponse();

            using (var con = new SqlConnection("[Data Source=db-mssql;Initial Catalog=s19183;Integrated Security=True]"))
            {
                using (var com = new SqlCommand())
                { 
                    com.Connection = con;
                    com.Parameters.AddWithValue("Semester", response.Semester);
                    com.Parameters.AddWithValue("Studies", response.Studies);
                    com.CommandText = "SELECT * FROM Enrollment e JOIN Studies s ON e.IdStudy=s.IdStudy WHERE e.Semester = @Semester AND s.Name=@Studies; ";
                    con.Open();
                    var transaction = con.BeginTransaction();
                    com.Transaction = transaction;

                    var dr = com.ExecuteReader();
                    if (dr.Read())
                    {
                        dr.Close();
                        com.CommandText = "MyProcedure";
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.ExecuteNonQuery();

                        result.Studies = response.Studies;
                        result.Semester = response.Semester + 1;
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest("Error");
                    }


                }

            }
        }
    }

}
