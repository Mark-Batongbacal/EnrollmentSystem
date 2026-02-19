using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EnrollmentSystem.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace EnrollmentSystem.Repository.Enrollments
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly EnrollmentSystemContext _context;

        public EnrollmentRepository(EnrollmentSystemContext context)
        {
            _context = context;
        }

        public async Task<Enrollment?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Enrollments
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EnrollmentId == id, cancellationToken);
        }

        public async Task<IEnumerable<Enrollment>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Enrollments
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Enrollment enrollment, CancellationToken cancellationToken = default)
        {
            await _context.Enrollments.AddAsync(enrollment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Enrollment enrollment, CancellationToken cancellationToken = default)
        {
            _context.Enrollments.Update(enrollment);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Enrollment enrollment, CancellationToken cancellationToken = default)
        {
            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Enrollments
                .AnyAsync(e => e.EnrollmentId == id, cancellationToken);
        }

        public async Task<bool> AnyByCourseOfferingIdAsync(int courseOfferingId, CancellationToken cancellationToken = default)
        {
            return await _context.Enrollments
                .AnyAsync(e => e.CourseOfferingId == courseOfferingId, cancellationToken);
        }

        public async Task<bool> AnyByStudentIdAsync(int studentId, CancellationToken cancellationToken = default)
        {
            return await _context.Enrollments
                .AnyAsync(e => e.StudentId == studentId, cancellationToken);
        }
    }
}
