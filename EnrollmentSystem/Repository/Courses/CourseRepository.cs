using EnrollmentSystem.Models.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EnrollmentSystem.Repository.Courses
{
    public class CourseRepository : ICourseRepository
    {
        private readonly EnrollmentSystemContext _context;

        public CourseRepository(EnrollmentSystemContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Course course)
        {
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return false;

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Courses.AnyAsync(c => c.CourseId == id);
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            return await _context.Courses.FindAsync(id);
        }

        public async Task Update(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }
    }
}
