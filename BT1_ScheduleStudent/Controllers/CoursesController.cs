using BT1_ScheduleStudent.Request;
using BT1_ScheduleStudent.Response;
using BT1_ScheduleStudent.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BT1_ScheduleStudent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly CourseService _CourseService;

        public CoursesController(CourseService CourseService)
        {
            _CourseService = CourseService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseRes>>> GetAllCourses()
        {
            var courses = await _CourseService.GetAllCoursesAsync();
            return Ok(courses);
        }

        // POST: api/Courses
        [HttpPost]
        public async Task<ActionResult<CourseRes>> PostCourse(CourseReq CourseReq)
        {
            var response = await _CourseService.CreateCourseAsync(CourseReq);
            return Ok();
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseRes>> GetCourse(int id)
        {
            var response = await _CourseService.GetCourseByIdAsync(id);

            if (response == null)
            {
                return NotFound();
            }

            return response;
        }

        // PUT: api/Courses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, CourseReq CourseReq)
        {
            var response = await _CourseService.UpdateCourseAsync(id, CourseReq);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var result = await _CourseService.DeleteCourseAsync(id);

            if (!result)
            {
                return NotFound(new { status = 0, decription = "Xóa thất bại" });
            }

            return Ok(new { status = 1, decription = "Xóa thành công" });
        }
    }
}
