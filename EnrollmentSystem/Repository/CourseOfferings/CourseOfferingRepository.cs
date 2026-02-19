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

        public async Task<bool> ExistsAsync(int courseId, int semesterId)
        {
            return await _context.CourseOfferings
                .AnyAsync(o => o.CourseId == courseId && o.SemesterId == semesterId);
        }
        public async Task<bool> IsFullAsync(int courseOfferingId)
        {
            var offering = await _context.CourseOfferings
                .Include(o => o.Enrollments)
                .FirstOrDefaultAsync(o => o.CourseOfferingId == courseOfferingId);
            if (offering == null) throw new KeyNotFoundException("Course offering not found.");
            return offering.Enrollments.Count >= offering.Capacity;
        }
        public async Task<bool> IsWithinSemesterAsync(int courseOfferingId, DateOnly date)
        {
            var offering = await _context.CourseOfferings
                .Include(o => o.Semester)
                .FirstOrDefaultAsync(o => o.CourseOfferingId == courseOfferingId);
            if (offering == null) throw new KeyNotFoundException("Course offering not found.");
            var semester = offering.Semester;
            return date >= semester.StartDate && date <= semester.EndDate;
        }
    }
}
