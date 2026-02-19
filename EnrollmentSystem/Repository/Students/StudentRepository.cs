using EnrollmentSystem.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace EnrollmentSystem.Repository.Students
{
    public class StudentRepository : IStudentRepository
    {
        private readonly EnrollmentSystemContext _context;

        public StudentRepository(EnrollmentSystemContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Student student)
        {
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.StudentId == id);
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Students
                .AnyAsync(s => s.StudentId == id);
        }
    }
}
