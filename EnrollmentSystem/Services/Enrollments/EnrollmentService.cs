using EnrollmentSystem.Models.Database;
using EnrollmentSystem.Repository.CourseOfferings;
using EnrollmentSystem.Repository.Enrollments;
using EnrollmentSystem.Repository.Logs;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;

namespace EnrollmentSystem.Services.Enrollments
{
    public class EnrollmentService: IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly ICourseOfferingRepository _courseOfferingRepo;
        private readonly ILogRepository _log;
        private readonly EnrollmentSystemContext _context;

        public EnrollmentService(
            ICourseOfferingRepository courseOfferingRepository,
            IEnrollmentRepository enrollmentRepository,
            ILogRepository log,
            EnrollmentSystemContext context)
        {
            _courseOfferingRepo = courseOfferingRepository;
            _enrollmentRepository = enrollmentRepository;
            _context = context;
            _log = log;
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
            // Check if student already enrolled in this offering
            if (await _enrollmentRepository.AnyByStudentIdAsync(studentId))
                throw new InvalidOperationException("Student is already enrolled in this course.");

            // Check if course offering is full
            if (await _enrollmentRepository.AnyByCourseOfferingIdAsync(courseOfferingId))
                throw new InvalidOperationException("Course is full.");

            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            if (!await _courseOfferingRepo.IsWithinSemesterAsync(courseOfferingId, today))
                throw new InvalidOperationException("Cannot enroll: semester has expired or not started yet.");



            // Create enrollment
            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseOfferingId = courseOfferingId,
                DateEnrolled = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            await _log.LogAsync(new Log
            {
                EnrollmentId = enrollment.EnrollmentId,
                StudentId = enrollment.StudentId,
                CourseOfferingId = enrollment.CourseOfferingId,
                FinalGrade = enrollment.FinalGrade,
                ActionType = "Student Enrolled",
                ActionDate = DateTime.UtcNow
            });

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

            await _log.LogAsync(new Log
            {
                EnrollmentId = enrollment.EnrollmentId,
                StudentId = enrollment.StudentId,
                CourseOfferingId = enrollment.CourseOfferingId,
                FinalGrade = enrollment.FinalGrade,
                ActionType = "Grade Updated",
                ActionDate = DateTime.UtcNow
            });

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var enrollment = await _enrollmentRepository.GetByIdAsync(id);

            if (enrollment == null)
                return false;

            await _context.Logs.AddAsync(new Log
            {
                EnrollmentId = enrollment.EnrollmentId,
                StudentId = enrollment.StudentId,
                CourseOfferingId = enrollment.CourseOfferingId,
                FinalGrade = enrollment.FinalGrade,
                ActionType = "Enrollment Deleted",
                ActionDate = DateTime.UtcNow
            });

            await _enrollmentRepository.DeleteAsync(enrollment);

            return true;
        }


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
