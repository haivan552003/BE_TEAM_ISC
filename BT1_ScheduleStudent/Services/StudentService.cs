namespace BT1_ScheduleStudent.Services
{
    public class StudentService
    {
        private readonly AppDbContext _context;

        public StudentService(AppDbContext context)
        {
            _context = context;
        }
    }
}
