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
            return Ok(new { status = 1, decription = response });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EnrollmentRes>> GetEnrollment(int id)
        {
            var response = await _EnrollmentService.GetEnrollmentByIdAsync(id);
            if(response == null)
            {
                return NotFound();
            }
            return Ok(new { status = 1, decription = response });
        }
        [HttpPost]
        public async Task<ActionResult<EnrollmentRes>> PostEnrollment(EnrollmentReq req)
        {
            var response = await _EnrollmentService.CreateEnrollmentAsync(req);
         return Ok(new { status = 1, decription = response });
        }
    }
}
