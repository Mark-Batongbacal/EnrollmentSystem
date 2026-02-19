using EnrollmentSystem.Models.Database;

namespace EnrollmentSystem.Repository.Courses
{
    public interface ICourseRepository
    {
        Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Course>> GetAllAsync(CancellationToken cancellationToken = default);

        Task AddAsync(Course course, CancellationToken cancellationToken = default);
        void Update(Course course);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
