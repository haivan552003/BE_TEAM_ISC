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

            return Ok("Cập nhật thành công");
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
