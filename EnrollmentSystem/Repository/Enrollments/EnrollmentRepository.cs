using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EnrollmentSystem.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace EnrollmentSystem.Repository.Enrollments
{
    public class EnrollmentRepository 
    {
        private readonly EnrollmentSystemContext _context;

        public EnrollmentRepository(EnrollmentSystemContext context)
        {
            _context = context;
        }

        public async Task<Enrollment?> GetByIdAsync(int id)
        {
            return await _context.Enrollments
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EnrollmentId == id);
        }

        public async Task<IEnumerable<Enrollment>> GetAllAsync()
        {
            return await _context.Enrollments
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(Enrollment enrollment)
        {
            await _context.Enrollments.AddAsync(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Enrollment enrollment)
        {
            _context.Enrollments.Update(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Enrollment enrollment)
        {
            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Enrollments
                .AnyAsync(e => e.EnrollmentId == id);
        }

        public async Task<bool> AnyByCourseOfferingIdAsync(int courseOfferingId)
        {
            return await _context.Enrollments
                .AnyAsync(e => e.CourseOfferingId == courseOfferingId);
        }

        public async Task<bool> AnyByStudentIdAsync(int studentId)
        {
            return await _context.Enrollments
                .AnyAsync(e => e.StudentId == studentId);
        }

    }
}
