using BT1_ScheduleStudent.Request;
using BT1_ScheduleStudent.Response;
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

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentRes>>> GetAllStudents()
        {
            var students = await _StudentService.GetAllStudentsAsync();
            return Ok(students);
        }

        // GET: api/Student/4
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentRes>> GetStudent(int id)
        {
            var response = await _StudentService.GetStudentByIdAsync(id);

            if (response == null)
            {
                return NotFound();
            }

            return response;
        }

        // POST: api/Student
        [HttpPost]
        public async Task<ActionResult<StudentRes>> PostStudent(StudentReq StudentReq)
        {
            var response = await _StudentService.CreateStudentAsync(StudentReq);
            return Ok();
        }

        
    }
}
