using EnrollmentSystem.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace EnrollmentSystem.Repository.CourseOfferings
{
    public class CourseOfferingRepository
    {
        private readonly EnrollmentSystemContext _context;

        public CourseOfferingRepository(EnrollmentSystemContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CourseOffering offering)
        {
            await _context.CourseOfferings.AddAsync(offering);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(CourseOffering offering)
        {
            _context.CourseOfferings.Remove(offering);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.CourseOfferings.AnyAsync(o => o.CourseId == id);
        }

        public async Task<IEnumerable<CourseOffering>> GetAllAsync()
        {
            return await _context.CourseOfferings.ToListAsync();
        }

        public async Task<IEnumerable<CourseOffering>> GetByCourseIdAsync(int courseId)
        {
            return await _context.CourseOfferings
                                 .Where(o => o.CourseId == courseId)
                                 .ToListAsync();
        }

        public async Task<CourseOffering?> GetByIdAsync(int id)
        {
            return await _context.CourseOfferings.FindAsync(id);
        }

        public async Task Update(CourseOffering offering)
        {
            _context.CourseOfferings.Update(offering);
            await _context.SaveChangesAsync();
        }
    }
}
