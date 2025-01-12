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
    }
}
