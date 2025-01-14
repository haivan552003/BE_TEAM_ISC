using BT1_ScheduleStudent.Request;
using BT1_ScheduleStudent.Response;
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnrollmentRes>>> GetAllEnrollments()
        {
            var response = await _EnrollmentService.GetAllEnrollmentsAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EnrollmentRes>> GetEnrollment(int id)
        {
            try
            {
                var response = await _EnrollmentService.GetEnrollmentByIdAsync(id);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
           
        }

        [HttpPost]
        public async Task<ActionResult<EnrollmentRes>> PostEnrollment(EnrollmentReq req)
        {
            try
            {
                var response = await _EnrollmentService.CreateEnrollmentAsync(req);
                return Ok(response);
            }           
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Đã xảy ra lỗi trong hệ thống.");
            }
        }
    }
}
