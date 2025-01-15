using BT1_ScheduleStudent.Response;
using BT1_ScheduleStudent.Request;
using BT1_ScheduleStudent.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BT1_ScheduleStudent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly EnrollmentService _EnrollmentService;

        public EnrollmentsController(EnrollmentService EnrollmentService)
        {
            _EnrollmentService = EnrollmentService;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEnrollment(int id, EnrollmentReq req)
        {
            var response = await _EnrollmentService.UpdateEnrollment(id, req);
            if (response.Status == 0)
            {
                return BadRequest(new { response.Status, response.Message });
            }
            return Ok(new { response.Status, response.Message, response.Data });
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<EnrollmentRes>> DeleteEnrollment(int id)
        {
            var result = await _EnrollmentService.DeleteEnrollment(id);

            if (!result)
            {
                return NotFound(new { status = 0, decription = "Xóa thất bại" });
            }

            return Ok(new { status = 1, decription = "Xóa thành công" });
        }
    }
}
