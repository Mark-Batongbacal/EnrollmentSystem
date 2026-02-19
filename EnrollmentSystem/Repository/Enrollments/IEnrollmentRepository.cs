using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EnrollmentSystem.Models.Database;

namespace EnrollmentSystem.Repository.Enrollments
{
    public interface IEnrollmentRepository
    {
        Task<Enrollment?> GetByIdAsync(int id);
        Task<IEnumerable<Enrollment>> GetAllAsync();

        Task AddAsync(Enrollment enrollment);
        Task UpdateAsync(Enrollment enrollment);
        Task DeleteAsync(Enrollment enrollment);

        Task<bool> ExistsAsync(int id);

        Task<bool> AnyByCourseOfferingIdAsync(int courseOfferingId);
        Task<bool> AnyByStudentIdAsync(int studentId);
    }
}
