using BT1_ScheduleStudent.Request;
using BT1_ScheduleStudent.Response;
using Microsoft.EntityFrameworkCore;

namespace BT1_ScheduleStudent.Services
{
    public class StudentService
    {
        private readonly AppDbContext _context;

        public StudentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<StudentRes> UpdateStudentAsync(int id, StudentReq Req)
        {
            if (Req == null)
            {
                throw new ArgumentNullException(nameof(Req), "Thông tin yêu cầu không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(Req.LastName))
            {
                throw new ArgumentException("Họ không được để trống.");
            }

            if (Req.LastName.Length > 15)
            {
                throw new ArgumentException("Họ không được dài quá 15 ký tự.");
            }

            if (!IsValidVietnameseName(Req.LastName))
            {
                throw new ArgumentException("Họ không chứa kí tự đặc biệt.");
            }

            if (string.IsNullOrWhiteSpace(Req.FirstMidName))
            {
                throw new ArgumentException("Tên đệm và tên không được để trống.");
            }

            if (Req.FirstMidName.Length > 20)
            {
                throw new ArgumentException("Tên đệm và tên không được dài quá 20 ký tự.");
            }

            if (!IsValidVietnameseName(Req.FirstMidName))
            {
                throw new ArgumentException("Tên đệm và tên không chứa kí tự đặc biệt.");
            }

            if (Req.EnrollmentDate == default(DateTime))
            {
                throw new ArgumentException("Ngày nhập học không được để trống hoặc không hợp lệ.");
            }

            var Student = await _context.Student
                .Where(e => e.StudentID == id)
                .FirstOrDefaultAsync();

            if (Student == null)
            {
                return null;
            }

            Student.LastName = Req.LastName;
            Student.FirstMidName = Req.FirstMidName;
            Student.EnrollmentDate = Req.EnrollmentDate;

            await _context.SaveChangesAsync();

            return new StudentRes();
        }

        private bool IsValidVietnameseName(string name)
        {
            var regex = new System.Text.RegularExpressions.Regex(@"^[\p{L}\s]+$");
            return regex.IsMatch(name);
        }

        // Delete Student
        public async Task<bool> DeleteStudentAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID không hợp lệ");
            }

            var Student = await _context.Student
                .Where(e => e.StudentID == id)
                .FirstOrDefaultAsync();

            if (Student == null)
            {
                return false;
            }

            _context.Student.Remove(Student);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
