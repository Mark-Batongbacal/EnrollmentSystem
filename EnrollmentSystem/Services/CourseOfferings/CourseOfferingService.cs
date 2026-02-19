using EnrollmentSystem.Models.Database;
using EnrollmentSystem.Repository.CourseOfferings;
using EnrollmentSystem.Repository.Enrollments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnrollmentSystem.Services.CourseOfferings
{
    public class CourseOfferingService : ICourseOfferingService
    {
        private readonly ICourseOfferingRepository _courseOfferingRepo;
        private readonly IEnrollmentRepository _enrollmentRepo;

        public CourseOfferingService(ICourseOfferingRepository courseOfferingRepo,
                                     IEnrollmentRepository enrollmentRepo)
        {
            _courseOfferingRepo = courseOfferingRepo;
            _enrollmentRepo = enrollmentRepo;
        }

        /// <summary>
        /// Adds a new course offering. Enforces uniqueness per course per semester.
        /// </summary>
        public async Task<CourseOffering> CreateAsync(CourseOffering courseOffering)
        {
            if (await _courseOfferingRepo.ExistsAsync(courseOffering.CourseId, courseOffering.SemesterId))
                throw new InvalidOperationException("CourseOffering must be unique per semester");

            await _courseOfferingRepo.AddAsync(courseOffering);
            return courseOffering;
        }


        /// <summary>
        /// Deletes a course offering if no enrollments exist.
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            var offering = await _courseOfferingRepo.GetByIdAsync(id);
            if (offering == null) return false;

            // Use the actual enrollment repository method
            var hasEnrollments = await _enrollmentRepo.AnyByCourseOfferingIdAsync(id);
            if (hasEnrollments)
                throw new InvalidOperationException("Cannot delete offering with enrollments");

            await _courseOfferingRepo.Delete(offering);
            return true;
        }

        /// <summary>
        /// Returns all course offerings.
        /// </summary>
        public Task<IEnumerable<CourseOffering>> GetAllAsync() =>
            _courseOfferingRepo.GetAllAsync();

        /// <summary>
        /// Gets a course offering by its ID.
        /// </summary>
        public Task<CourseOffering?> GetByIdAsync(int id) =>
            _courseOfferingRepo.GetByIdAsync(id);

        /// <summary>
        /// Checks if a course offering has any enrollments.
        /// </summary>
        public Task<bool> HasEnrollmentsAsync(int courseOfferingId) =>
            _enrollmentRepo.AnyByCourseOfferingIdAsync(courseOfferingId);

        /// <summary>
        /// Checks if a course offering is full.
        /// </summary>
        public Task<bool> IsFullAsync(int courseOfferingId) =>
            _courseOfferingRepo.IsFullAsync(courseOfferingId);

        /// <summary>
        /// Checks if a course offering is within the given semester dates.
        /// </summary>
        public Task<bool> IsWithinSemesterAsync(int courseOfferingId, DateTime date) =>
            _courseOfferingRepo.IsWithinSemesterAsync(courseOfferingId, date);

        /// <summary>
        /// Updates an existing course offering.
        /// </summary>
        public async Task<bool> UpdateAsync(int id, CourseOffering courseOffering)
        {
            var existing = await _courseOfferingRepo.GetByIdAsync(id);
            if (existing == null) return false;

            await _courseOfferingRepo.Update(courseOffering);
            return true;
        }
    }
}
