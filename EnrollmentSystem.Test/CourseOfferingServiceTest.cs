using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnrollmentSystem.Models.Database;
using EnrollmentSystem.Repository.CourseOfferings;
using EnrollmentSystem.Repository.Enrollments;
using EnrollmentSystem.Services.CourseOfferings;
using Moq;

namespace EnrollmentSystem.Test
{
    public class CourseOfferingServiceTest
    {
        private readonly Mock<ICourseOfferingRepository> _courseOfferingRepo;
        private readonly Mock<IEnrollmentRepository> _enrollmentRepo;
        private readonly CourseOfferingService _service;

        public CourseOfferingServiceTest()
        {
            _courseOfferingRepo = new Mock<ICourseOfferingRepository>();
            _enrollmentRepo = new Mock<IEnrollmentRepository>(); 
            _service = new CourseOfferingService(_courseOfferingRepo.Object, _enrollmentRepo.Object);
        }


        [Fact]
        public async Task CourseOffering_ShouldBeUnique_PerSemester()
        {
            // Arrange
            var offering = new CourseOffering { CourseId = 1, SemesterId = 20251 };
            _courseOfferingRepo.Setup(r => r.ExistsAsync(offering.CourseId, offering.SemesterId)).ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(offering));
        }

        [Fact]
        public async Task CreateOffering_ShouldFail_WhenDuplicateCourseSemester()
        {
            // Arrange
            var offering = new CourseOffering { CourseId = 2, SemesterId = 20252 };
            _courseOfferingRepo.Setup(r => r.ExistsAsync(offering.CourseId, offering.SemesterId)).ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(offering));
        }

        
    }
}
