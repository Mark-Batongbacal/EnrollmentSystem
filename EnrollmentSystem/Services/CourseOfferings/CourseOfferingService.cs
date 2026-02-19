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

        public async Task<CourseOffering> CreateAsync(CourseOffering courseOffering)
        {
            if (await _courseOfferingRepo.ExistsAsync(courseOffering.CourseId, courseOffering.SemesterId))
                throw new InvalidOperationException("CourseOffering must be unique per semester");

            await _courseOfferingRepo.AddAsync(courseOffering);
            return courseOffering;
        }


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

        public Task<IEnumerable<CourseOffering>> GetAllAsync() =>
            _courseOfferingRepo.GetAllAsync();

        public Task<CourseOffering?> GetByIdAsync(int id) =>
            _courseOfferingRepo.GetByIdAsync(id);

        public Task<bool> HasEnrollmentsAsync(int courseOfferingId) =>
            _enrollmentRepo.AnyByCourseOfferingIdAsync(courseOfferingId);

        public Task<bool> IsFullAsync(int courseOfferingId) =>
            _courseOfferingRepo.IsFullAsync(courseOfferingId);

        public Task<bool> IsWithinSemesterAsync(int courseOfferingId, DateOnly date) =>
            _courseOfferingRepo.IsWithinSemesterAsync(courseOfferingId, date);

        public async Task<bool> UpdateAsync(int id, CourseOffering courseOffering)
        {
            var existing = await _courseOfferingRepo.GetByIdAsync(id);
            if (existing == null) return false;

            await _courseOfferingRepo.Update(courseOffering);
            return true;
        }
    }
}
