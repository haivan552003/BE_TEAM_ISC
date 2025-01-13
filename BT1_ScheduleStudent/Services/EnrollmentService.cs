using System.Numerics;
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

        public async Task<IEnumerable<EnrollmentRes>> GetAllEnrollmentsAsync()
        {
            var enrollments = await _context.Enrollment.ToListAsync();

            var enrollmentResponses = enrollments.Select(results => new EnrollmentRes
            {
                CourseID = results.CourseID,
                EnrollmentID = results.EnrollmentID,
                Grade = results.Grade,
                StudentID = results.StudentID,
            }).ToList();

            return enrollmentResponses;
        }

        public async Task<EnrollmentRes> GetEnrollmentByIdAsync(int id)
        {
            var results = await _context.Enrollment
                .Where(e => e.EnrollmentID == id)
                .FirstOrDefaultAsync();
            if (results == null)
            {
                return null;
            }
            else
            {
                return new EnrollmentRes
                {
                    CourseID = results.CourseID,
                    EnrollmentID = results.EnrollmentID,
                    Grade = results.Grade,
                    StudentID = results.StudentID,
                };
            }
        }

        public async Task<EnrollmentRes> CreateEnrollmentAsync(EnrollmentReq req)
        {
            ValidateEnrollmentRequest(req);
            var courseExist = await _context.Course
                              .AnyAsync(course => course.CourseID == req.CourseID);
            if (!courseExist)
            {
                throw new ArgumentException($"Course with Id {req.CourseID} does not exist");
            }
            var studentExist = await _context.Student
                                .AnyAsync(student => student.StudentID == req.StudentID);
            if (!studentExist)
            {
                throw new ArgumentException($"Student with Id {req.StudentID} does not exist");
            }
            var enrollment = new Enrollment
            {
                CourseID = req.CourseID,
                StudentID = req.StudentID,
                Grade = req.Grade,
            };

            _context.Enrollment.Add(enrollment);
            await _context.SaveChangesAsync();

            return new EnrollmentRes
            {
                EnrollmentID = enrollment.EnrollmentID,
                CourseID = enrollment.CourseID,
                StudentID = enrollment.StudentID,
                Grade = enrollment.Grade,
            };
        }

        private void ValidateEnrollmentRequest(EnrollmentReq req)
        {
            if (req == null)
            {
                throw new FormatException("Dữ liệu yêu cầu không được để trống.");
            }

            //if (!int.TryParse(req.CourseID.ToString(), out _))
            //{
            //    throw new FormatException("CourseID phải là một số nguyên hợp lệ.");
            //}

            //if (!int.TryParse(req.StudentID.ToString(), out _))
            //{
            //    throw new FormatException("StudentID phải là một số nguyên hợp lệ.");
            //}

            if (string.IsNullOrWhiteSpace(req.Grade))
            {
                throw new ArgumentException("Grade không được bỏ trống.", nameof(req.Grade));
            }

            if (req.Grade.Length >= 10)
            {
                throw new ArgumentException("Grade không được vượt quá 10 ký tự.", nameof(req.Grade));
            }
        }


    }
}
