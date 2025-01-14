using BT1_ScheduleStudent.Request;
using BT1_ScheduleStudent.Response;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<object> UpdateStudentAsync(int id, StudentReq Req)
        {
            if (Req == null)
            {
                return "Thông tin yêu cầu không được để trống."; 
            }

            if (string.IsNullOrWhiteSpace(Req.LastName))
            {
                return "Họ không được để trống.";
            }

            if (Req.LastName.Length > 15)
            {
                return "Họ không được dài quá 15 ký tự."; 
            }

            if (!IsValidVietnameseName(Req.LastName))
            {
                return "Họ không chứa kí tự đặc biệt.";
            }

            if (string.IsNullOrWhiteSpace(Req.FirstMidName))
            {
                return "Tên đệm và tên không được để trống.";
            }

            if (Req.FirstMidName.Length > 20)
            {
                return "Tên đệm và tên không được dài quá 20 ký tự.";
            }

            if (!IsValidVietnameseName(Req.FirstMidName))
            {
                return "Tên đệm và tên không chứa kí tự đặc biệt.";
            }

            if (Req.EnrollmentDate == default(DateTime))
            {
                return "Ngày nhập học không được để trống hoặc không hợp lệ.";
            }

            var Student = await _context.Student
                .Where(e => e.StudentID == id)
                .FirstOrDefaultAsync();

            if (Student == null)
            {
                return "Không tìm thấy sinh viên với ID này.";
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
