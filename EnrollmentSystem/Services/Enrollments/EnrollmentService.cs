using EnrollmentSystem.Models.Database;
using EnrollmentSystem.Repository.Enrollments;
using Microsoft.EntityFrameworkCore;

namespace EnrollmentSystem.Services.Enrollments
{
    public class EnrollmentService: IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly EnrollmentSystemContext _context;

        public EnrollmentService(
            IEnrollmentRepository enrollmentRepository,
            EnrollmentSystemContext context)
        {
            _enrollmentRepository = enrollmentRepository;
            _context = context;
        }

        public async Task<IEnumerable<Enrollment>> GetAllAsync()
        {
            return await _enrollmentRepository.GetAllAsync();
        }

        public async Task<Enrollment?> GetByIdAsync(int id)
        {
            return await _enrollmentRepository.GetByIdAsync(id);
        }

        public async Task<Enrollment> EnrollAsync(int studentId, int courseOfferingId)
        {
            if (await IsStudentEnrolledAsync(studentId, courseOfferingId))
                throw new InvalidOperationException("Student is already enrolled in this course.");

            var courseOffering = await _context.CourseOfferings
                .Include(c => c.Enrollments)
                .FirstOrDefaultAsync(c => c.CourseOfferingId == courseOfferingId);

            if (courseOffering == null)
                throw new KeyNotFoundException("Course offering not found.");

            if (courseOffering.Capacity <= courseOffering.Enrollments.Count)
                throw new InvalidOperationException("Course is full.");

            var semester = await _context.Semesters
                .FirstOrDefaultAsync(s => s.SemesterId == courseOffering.SemesterId);

            if (semester == null)
                throw new KeyNotFoundException("Semester not found.");

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            if (today < semester.StartDate || today > semester.EndDate)
                throw new InvalidOperationException("Enrollment is outside semester dates.");

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseOfferingId = courseOfferingId,
                FinalGrade = null,
                DateEnrolled = today
            };

            await _enrollmentRepository.AddAsync(enrollment);

            return enrollment;
        }

        public async Task<bool> UpdateGradeAsync(int enrollmentId, int grade)
        {
            if (!IsValidGrade(grade))
                throw new ArgumentException("Grade must be between 0 and 100.");

            var enrollment = await _enrollmentRepository.GetByIdAsync(enrollmentId);

            if (enrollment == null)
                return false;

            enrollment.FinalGrade = grade;

            await _enrollmentRepository.UpdateAsync(enrollment);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var enrollment = await _enrollmentRepository.GetByIdAsync(id);

            if (enrollment == null)
                return false;

            await _enrollmentRepository.DeleteAsync(enrollment);

            return true;
        }

        // Business Rules

        public async Task<bool> IsStudentEnrolledAsync(int studentId, int courseOfferingId)
        {
            return await _context.Enrollments
                .AnyAsync(e =>
                    e.StudentId == studentId &&
                    e.CourseOfferingId == courseOfferingId);
        }

        public bool IsValidGrade(int grade)
        {
            return grade >= 0 && grade <= 100;
        }
    }
}
