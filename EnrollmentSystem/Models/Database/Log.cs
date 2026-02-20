namespace EnrollmentSystem.Models.Database
{
    public class Log
    {
        public int HistoryId { get; set; }

        public int? EnrollmentId { get; set; }

        public int? StudentId { get; set; }

        public int? CourseOfferingId { get; set; }

        public int? FinalGrade { get; set; }

        public string? ActionType { get; set; }

        public DateTime? ActionDate { get; set; }
    }
}
