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
                throw new ArgumentNullException(nameof(Req), "Request data is required.");
            }

            if (string.IsNullOrWhiteSpace(Req.LastName))
            {
                throw new ArgumentException("LastName is required.");
            }

            if (string.IsNullOrWhiteSpace(Req.FirstMidName))
            {
                throw new ArgumentException("FirstMidName is required.");
            }

            if (Req.EnrollmentDate == default)
            {
                throw new ArgumentException("EnrollmentDate is invalid.");
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

        // Delete Course
        public async Task<bool> DeleteStudentAsync(int id)
        {
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
