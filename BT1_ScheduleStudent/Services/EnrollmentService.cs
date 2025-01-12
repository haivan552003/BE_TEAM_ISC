namespace BT1_ScheduleStudent.Services
{
    public class EnrollmentService
    {
        private readonly AppDbContext _context;

        public EnrollmentService(AppDbContext context)
        {
            _context = context;
        }
    }
}
