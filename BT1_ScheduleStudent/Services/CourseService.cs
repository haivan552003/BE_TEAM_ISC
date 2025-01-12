using BT1_ScheduleStudent.Model;
using BT1_ScheduleStudent.Request;
using BT1_ScheduleStudent.Response;
using Microsoft.EntityFrameworkCore;

namespace BT1_ScheduleStudent.Services
{
    public class CourseService
    {
        private readonly AppDbContext _context;

        public CourseService(AppDbContext context)
        {
            _context = context;
        }

        // Get All Course
        public async Task<IEnumerable<CourseRes>> GetAllCoursesAsync()
        {
            var enrollments = await _context.Course.ToListAsync();

            var enrollmentResponses = enrollments.Select(enrollment => new CourseRes
            {
                CourseID = enrollment.CourseID,
                Title = enrollment.Title,
                Creadits = enrollment.Creadits
            }).ToList();

            return enrollmentResponses;
        }


        // Create Course
        public async Task<CourseRes> CreateCourseAsync(CourseReq Req)
        {
            var Course = new Course
            {
                Title = Req.Title,
                Creadits = Req.Creadits,
            };

            _context.Course.Add(Course);
            await _context.SaveChangesAsync();

            return new CourseRes();
        }

        // Get Course by ID
        public async Task<CourseRes> GetCourseByIdAsync(int id)
        {
            var Course = await _context.Course
                .Where(e => e.CourseID == id)
                .FirstOrDefaultAsync();

            if (Course == null)
            {
                return null;
            }

            return new CourseRes
            {
                CourseID = Course.CourseID,
                Title = Course.Title,
                Creadits = Course.Creadits,
            };
        }

        // Update Course
        public async Task<CourseRes> UpdateCourseAsync(int id, CourseReq Req)
        {
            var Course = await _context.Course
                .Where(e => e.CourseID == id)
                .FirstOrDefaultAsync();

            if (Course == null)
            {
                return null;
            }

            Course.Title = Req.Title;
            Course.Creadits = Req.Creadits;

            await _context.SaveChangesAsync();

            return new CourseRes();
        }

        // Delete Course
        public async Task<bool> DeleteCourseAsync(int id)
        {
            var Course = await _context.Course
                .Where(e => e.CourseID == id)
                .FirstOrDefaultAsync();

            if (Course == null)
            {
                return false;
            }

            _context.Course.Remove(Course);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
