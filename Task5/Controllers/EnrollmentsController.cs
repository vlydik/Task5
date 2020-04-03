using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Task5.Controllers
{
    [Route("api/enrollments")]
    [ApiController] //implicit model validation
    public class EnrollmentsController : ControllerBase
    {
        [HttpPost]
        public IActionResult EnrollStudent()
        {

            return Ok();
        }
    }
}