using EnrollmentSystem.Models.Database;
using EnrollmentSystem.Repository.Students;

namespace EnrollmentSystem.Services.Students
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;

        public StudentService(IStudentRepository repository)
        {
            _repository = repository;
        }

        public async Task<Student> CreateAsync(Student student)
        {
            await _repository.AddAsync(student);
            return student;
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Student>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Student?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(int id, Student student)
        {
            throw new NotImplementedException();
        }
    }
}
