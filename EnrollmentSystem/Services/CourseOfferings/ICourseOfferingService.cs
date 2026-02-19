using EnrollmentSystem.Models.Database;
namespace EnrollmentSystem.Services.CourseOfferings
{
    public interface ICourseOfferingService
    {
        Task<IEnumerable<CourseOffering>> GetAllAsync();
        Task<CourseOffering?> GetByIdAsync(int id);
        Task<CourseOffering> CreateAsync(CourseOffering courseOffering);
        Task<bool> UpdateAsync(int id, CourseOffering courseOffering);
        Task<bool> DeleteAsync(int id);

        // Business Rule Helpers
        Task<bool> IsFullAsync(int courseOfferingId);
        Task<bool> HasEnrollmentsAsync(int courseOfferingId);
        Task<bool> IsWithinSemesterAsync(int courseOfferingId, DateTime date);
    }
}
