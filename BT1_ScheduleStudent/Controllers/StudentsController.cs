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
            if (response is string errorMessage)
            {
                return BadRequest(errorMessage);
            }
            return Ok("Thêm thành công");
        }

        
        // PUT: api/Student/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, StudentReq StudentReq)
        {
            var response = await _StudentService.UpdateStudentAsync(id, StudentReq);

            if (response == null)
            {
                return NotFound();
            }

            if (!(response is StudentRes))
            {
                return BadRequest(response);
            }

            return Ok(response);
        }


        // DELETE: api/Student/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var result = await _StudentService.DeleteStudentAsync(id);

            if (!result)
            {
                return NotFound(new { status = 0, decription = "Xóa thất bại" });
            }

            return Ok(new { status = 1, decription = "Xóa thành công" });
        }
    }
}
