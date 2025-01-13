using BT1_ScheduleStudent.Model;
using BT1_ScheduleStudent.Request;
using BT1_ScheduleStudent.Response;
using Microsoft.EntityFrameworkCore;

namespace BT1_ScheduleStudent.Services
{
    public class EnrollmentService
    {
        private readonly AppDbContext _context;

        public EnrollmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EnrollmentRes> UpdateEnrollment(int id, EnrollmentReq req)
        {
            ValidateEnrollmentRequest(req);

            var enrollmentExist = await _context.Enrollment
            .FirstOrDefaultAsync(e => e.EnrollmentID == id);

            if (enrollmentExist == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy Enrollment với Id {id}.");
            }

            var courseExist = await _context.Course
                .AnyAsync(course => course.CourseID == req.CourseID);
            if (!courseExist)
            {
                throw new KeyNotFoundException($"Không tìm thấy Course với Id {req.CourseID}.");
            }

            var studentExist = await _context.Student
                .AnyAsync(student => student.StudentID == req.StudentID);
            if (!studentExist)
            {
                throw new KeyNotFoundException($"Không tìm thấy Student với Id {req.StudentID}.");
            }

            enrollmentExist.StudentID = req.StudentID;
            enrollmentExist.CourseID = req.CourseID;
            enrollmentExist.Grade = req.Grade;

            await _context.SaveChangesAsync();

            return new EnrollmentRes {
                EnrollmentID = enrollmentExist.EnrollmentID,
                CourseID = enrollmentExist.CourseID,
                StudentID = enrollmentExist.StudentID,
                Grade = enrollmentExist.Grade
            };
        }

        public async Task<bool> DeleteEnrollment(int id)
        {
            var enrollmentExist = await _context.Enrollment
                 .Where(e => e.EnrollmentID == id)
                 .FirstOrDefaultAsync();
            if (enrollmentExist == null)
            {
                return false;
            }

            _context.Enrollment.Remove(enrollmentExist);

            await _context.SaveChangesAsync();

            return true;
        }

        private void ValidateEnrollmentRequest(EnrollmentReq req)
        {
            if (req == null)
            {
                throw new FormatException("Dữ liệu yêu cầu không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(req.Grade))
            {
                throw new ArgumentException("Grade không được bỏ trống.", nameof(req.Grade));
            }

            if (req.Grade.Length > 10)
            {
                throw new ArgumentException("Grade không được vượt quá 10 ký tự.", nameof(req.Grade));
            }
        }

    }
}
