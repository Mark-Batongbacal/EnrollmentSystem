using EnrollmentSystem.Models.Database;

namespace EnrollmentSystem.Repository.Courses
{
    public interface ICourseRepository
    {
        Task<Course?> GetByIdAsync(int id);
        Task<IEnumerable<Course>> GetAllAsync();

        Task AddAsync(Course course);
        Task Update(Course course);
        Task<bool> DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);
    }
}
