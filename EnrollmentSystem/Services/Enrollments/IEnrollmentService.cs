using EnrollmentSystem.Models.Database;

namespace EnrollmentSystem.Services.Enrollments
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<Enrollment>> GetAllAsync();
        Task<Enrollment?> GetByIdAsync(int id);
        Task<Enrollment> EnrollAsync(int studentId, int courseOfferingId);
        Task<bool> UpdateGradeAsync(int enrollmentId, int grade);
        Task<bool> DeleteAsync(int id);

        // Business Rules
        Task<bool> IsStudentEnrolledAsync(int studentId, int courseOfferingId);
        bool IsValidGrade(int grade);
    }
}
