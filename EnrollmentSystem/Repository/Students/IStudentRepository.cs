using EnrollmentSystem.Models.Database;

namespace EnrollmentSystem.Repository.Students
{
    public interface IStudentRepository
    {
        Task<Student?> GetByIdAsync(int id);
        Task<IEnumerable<Student>> GetAllAsync();

        Task AddAsync(Student student);
        void Update(Student student);
        void Delete(Student student);

        Task<bool> ExistsAsync(int id);
    }
}

