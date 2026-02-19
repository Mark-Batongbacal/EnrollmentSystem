using EnrollmentSystem.Models.Database;
using EnrollmentSystem.Repository.Students;

namespace EnrollmentSystem.Services.Students
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _studentRepository.GetAllAsync();
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _studentRepository.GetByIdAsync(id);
        }

        public async Task<Student> CreateAsync(Student student)
        {
            student.DateCreated = DateOnly.FromDateTime(DateTime.UtcNow);

            if (_studentRepository.ExistsAsync(student.StudentNumber).Result)
            {
                throw new InvalidOperationException($"A student with StudentNumber {student.StudentNumber} already exists.");
            }
            await _studentRepository.AddAsync(student);

            return student;
        }

        public async Task<bool> UpdateAsync(int id, Student updatedStudent)
        {
            var existingStudent = await _studentRepository.GetByIdAsync(id);

            if (existingStudent == null)
                return false;

            existingStudent.StudentNumber = updatedStudent.StudentNumber;
            existingStudent.FirstName = updatedStudent.FirstName;
            existingStudent.LastName = updatedStudent.LastName;
            existingStudent.Email = updatedStudent.Email;

            await _studentRepository.Update(existingStudent);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);

            if (student == null)
                return false;

            await _studentRepository.Delete(student);

            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _studentRepository.ExistsAsync(id);
        }
    }
}