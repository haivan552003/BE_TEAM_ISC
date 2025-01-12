using System.ComponentModel.DataAnnotations;

namespace BT1_ScheduleStudent.Request
{
    public class EnrollmentReq
    {
        public int CourseID { get; set; }
        public int StudentID { get; set; }

        [Required(ErrorMessage = "Grade not null")]
        public string Grade { get; set; }
    }
}
