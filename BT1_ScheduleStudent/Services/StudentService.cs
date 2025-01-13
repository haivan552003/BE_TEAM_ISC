using BT1_ScheduleStudent.Model;
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

        // Get All Student
        public async Task<IEnumerable<StudentRes>> GetAllStudentsAsync()
        {
            var enrollments = await _context.Student.ToListAsync();

            var enrollmentResponses = enrollments.Select(enrollment => new StudentRes
            {
                StudentID = enrollment.StudentID,
                LastName = enrollment.LastName,
                FirstMidName = enrollment.FirstMidName,
                EnrollmentDate = enrollment.EnrollmentDate
            }).ToList();

            return enrollmentResponses;
        }

        // Get ID Student
        public async Task<StudentRes> GetStudentByIdAsync(int id)
        {
            var Student = await _context.Student
                .Where(e => e.StudentID == id)
                .FirstOrDefaultAsync();

            if (Student == null)
            {
                return null;
            }

            return new StudentRes
            {
                StudentID = Student.StudentID,
                LastName = Student.LastName,
                FirstMidName = Student.FirstMidName,
                EnrollmentDate = Student.EnrollmentDate
            };
        }

        // Create Student
        public async Task<StudentRes> CreateStudentAsync(StudentReq Req)
        {
            var Student = new Student
            {
                LastName = Req.LastName,
                FirstMidName = Req.FirstMidName,
                EnrollmentDate = Req.EnrollmentDate
            };

            _context.Student.Add(Student);
            await _context.SaveChangesAsync();

            return new StudentRes();
        }
    }
}
