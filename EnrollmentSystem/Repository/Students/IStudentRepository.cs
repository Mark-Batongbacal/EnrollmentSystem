using EnrollmentSystem.Models.Database;

namespace EnrollmentSystem.Repository.Students
{
    public interface IStudentRepository
    {
        Task<Student?> GetByIdAsync(int id);
        Task<IEnumerable<Student>> GetAllAsync();

        Task AddAsync(Student student);
        Task Update(Student student);
        Task Delete(Student student);

        Task<bool> ExistsAsync(int id);
    }
}

