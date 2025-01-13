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
            if (id <= 0)
            {
                throw new ArgumentException("ID không hợp lệ.");
            }
            var results = await _context.Enrollment
                .Where(e => e.EnrollmentID == id)
                .FirstOrDefaultAsync();
            if (results == null)
            {
               throw new InvalidOperationException($"Id {id} không tồn tại.");
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
                throw new ArgumentException($"CourseId {req.CourseID} không tồn tại.");
            }
            var studentExist = await _context.Student
                                .AnyAsync(student => student.StudentID == req.StudentID);
            if (!studentExist)
            {
                throw new ArgumentException($"StudentId {req.StudentID} Không tồn tại.");
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
                throw new ArgumentException("Dữ liệu yêu cầu không được để trống.");
            }
            if (req.CourseID <= 0)
            {
                throw new ArgumentException("CourseID không hợp lệ.");
            }
            if (req.StudentID <= 0)
            {
                throw new ArgumentException("StudentID không hợp lệ.");
            }
            //if (!int.TryParse(req.CourseID.ToString(), out _))
            //{
            //    throw new ArgumentException("CourseID phải là một số nguyên hợp lệ.");
            //}

            //if (!int.TryParse(req.StudentID.ToString(), out _))
            //{
            //    throw new ArgumentException("StudentID phải là một số nguyên hợp lệ.");
            //}

            if (string.IsNullOrWhiteSpace(req.Grade))
            {
                throw new ArgumentException("Grade không được bỏ trống.");
            }

            if (req.Grade.Length >= 10)
            {
                throw new ArgumentException("Grade không được vượt quá 10 ký tự.");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(req.Grade, @"^[a-zA-Z0-9]+$"))
            {
                throw new ArgumentException("Grade chỉ được chứa chữ cái và chữ số, không được chứa ký tự đặc biệt.");
            }
        }



        public async Task<EnrollmentRes> UpdateEnrollment(int id, EnrollmentReq req)
        {
            //Tìm xem có tồn tại ID Enrollment?
            var enrollmentExist = await _context.Enrollment
                 .Where(e => e.EnrollmentID == id)
                 .FirstOrDefaultAsync();
            if (enrollmentExist == null)
            {
                throw new ArgumentException("Enrollment does not exits");
            }

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

            enrollmentExist.StudentID = req.StudentID;
            enrollmentExist.CourseID = req.CourseID;
            enrollmentExist.Grade = req.Grade;

            //Lưu thay đổi
            await _context.SaveChangesAsync();

            return new EnrollmentRes();
        }

        public async Task<bool> DeleteEnrollment(int id)
        {
            //Tìm xem có tồn tại ID Enrollment?
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
    }
}
