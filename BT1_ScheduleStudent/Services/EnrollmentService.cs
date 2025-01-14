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

        public class ResponseWrapper<T>
        {
            public int Status { get; set; }
            public string Message { get; set; }
            public T Data { get; set; }
        }

        public async Task<ResponseWrapper<EnrollmentRes>> UpdateEnrollment(int id, EnrollmentReq req)
        {
            try
            {
                ValidateEnrollmentRequest(req);

                var enrollmentExist = await _context.Enrollment
                    .FirstOrDefaultAsync(e => e.EnrollmentID == id);

                if (enrollmentExist == null)
                {
                    return new ResponseWrapper<EnrollmentRes>
                    {
                        Status = 0,
                        Message = $"Không tìm thấy Enrollment với Id {id}."
                    };
                }

                var courseExist = await _context.Course
                    .AnyAsync(course => course.CourseID == req.CourseID);
                if (!courseExist)
                {
                    return new ResponseWrapper<EnrollmentRes>
                    {
                        Status = 0,
                        Message = $"Không tìm thấy Course với Id {req.CourseID}."
                    };
                }

                var studentExist = await _context.Student
                    .AnyAsync(student => student.StudentID == req.StudentID);
                if (!studentExist)
                {
                    return new ResponseWrapper<EnrollmentRes>
                    {
                        Status = 0,
                        Message = $"Không tìm thấy Student với Id {req.StudentID}."
                    };
                }

                enrollmentExist.StudentID = req.StudentID;
                enrollmentExist.CourseID = req.CourseID;
                enrollmentExist.Grade = req.Grade;

                await _context.SaveChangesAsync();

                return new ResponseWrapper<EnrollmentRes>
                {
                    Status = 1,
                    Message = "Cập nhật thành công",
                    Data = new EnrollmentRes
                    {
                        EnrollmentID = enrollmentExist.EnrollmentID,
                        CourseID = enrollmentExist.CourseID,
                        StudentID = enrollmentExist.StudentID,
                        Grade = enrollmentExist.Grade
                    }
                };
            }
            catch (Exception ex)
            {
                return new ResponseWrapper<EnrollmentRes>
                {
                    Status = 0,
                    Message = $"Đã xảy ra lỗi: {ex.Message}"
                };
            }
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
                throw new ArgumentException("Dữ liệu yêu cầu không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(req.Grade))
            {
                throw new ArgumentException("Grade không được bỏ trống.");
            }

            if (req.Grade.Length > 10)
            {
                throw new ArgumentException("Grade không được vượt quá 10 ký tự.");
            }
        }
    }
}
