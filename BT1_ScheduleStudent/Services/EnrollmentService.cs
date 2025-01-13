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
