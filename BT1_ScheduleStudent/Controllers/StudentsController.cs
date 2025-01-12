using BT1_ScheduleStudent.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BT1_ScheduleStudent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentService _StudentService;

        public StudentsController(StudentService StudentService)
        {
            _StudentService = StudentService;
        }
    }
}
