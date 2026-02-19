using EnrollmentSystem.Models.Database;

namespace EnrollmentSystem.Repository.CourseOfferings
{
    public interface ICourseOfferingRepository
    {
        Task<CourseOffering?> GetByIdAsync(int id);
        Task<IEnumerable<CourseOffering>> GetAllAsync();

        Task AddAsync(CourseOffering offering);
        Task Update(CourseOffering offering);
        Task Delete(CourseOffering offering);

        Task<bool> ExistsAsync(int id);

        Task<IEnumerable<CourseOffering>> GetByCourseIdAsync(int courseId);

        Task<bool> ExistsAsync(int courseId, int semesterId);
        Task<bool> IsFullAsync(int courseOfferingId);
        Task<bool> IsWithinSemesterAsync(int courseOfferingId, DateOnly date);
    }
}

