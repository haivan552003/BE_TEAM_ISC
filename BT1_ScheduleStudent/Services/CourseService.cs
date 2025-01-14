using BT1_ScheduleStudent.Model;
using BT1_ScheduleStudent.Request;
using BT1_ScheduleStudent.Response;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

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
        public async Task<object> CreateCourseAsync(CourseReq Req)
        {
            if (string.IsNullOrWhiteSpace(Req.Title))
            {
                return "Title không được bỏ trống.";
            }

            if (Req.Title.Length > 250)
            {
                return "Title quá dài. Tối đa 10 ký tự.";
            }

            if (!Regex.IsMatch(Req.Title, @"^[a-zA-Z0-9\s]+$"))
            {
                return "Title không được chứa ký tự đặc biệt.";
            }

            if (string.IsNullOrWhiteSpace(Req.Creadits))
            {
                return "Số thẻ ngân hàng không được bỏ trống.";
            }

            if (!Regex.IsMatch(Req.Creadits, @"^\d+$"))
            {
                return "Số thẻ ngân hàng chỉ được chứa số.";
            }

            if (Req.Creadits.Length != 16 && Req.Creadits.Length != 19)
            {
                return "Số thẻ ngân hàng phải gồm 16 hoặc 19 chữ số.";
            }

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
        public async Task<object> UpdateCourseAsync(int id, CourseReq Req)
        {
            if (string.IsNullOrWhiteSpace(Req.Title))
            {
                return "Title không được bỏ trống.";
            }

            if (Req.Title.Length > 250)
            {
                return "Title quá dài.";
            }

            if (!Regex.IsMatch(Req.Title, @"^[a-zA-Z0-9\s]+$"))
            {
                return "Title không được chứa ký tự đặc biệt.";
            }

            if (string.IsNullOrWhiteSpace(Req.Creadits))
            {
                return "Số thẻ ngân hàng không được bỏ trống.";
            }

            if (!Regex.IsMatch(Req.Creadits, @"^\d+$"))
            {
                return "Số thẻ ngân hàng chỉ được chứa số.";
            }

            if (Req.Creadits.Length != 16 && Req.Creadits.Length != 19)
            {
                return "Số thẻ ngân hàng phải gồm 16 hoặc 19 chữ số.";
            }

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
