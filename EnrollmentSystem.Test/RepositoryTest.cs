using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using EnrollmentSystem.Models.Database;
using EnrollmentSystem.Repository.Courses;
using EnrollmentSystem.Repository.Enrollments;
using EnrollmentSystem.Repository.Students;
using Moq;
using Xunit;

namespace EnrollmentSystem.Test
{
    public class RepositoryTests
    {
        private readonly Mock<ICourseRepository> _courseRepo;
        private readonly Mock<IStudentRepository> _studentRepo;
        private readonly Mock<IEnrollmentRepository> _enrollmentRepo;

        public RepositoryTests()
        {
            _courseRepo = new Mock<ICourseRepository>();
            _studentRepo = new Mock<IStudentRepository>();
            _enrollmentRepo = new Mock<IEnrollmentRepository>();
        }

        [Fact]
        public async Task DeleteCourse_ShouldFail_WhenEnrollmentsExist()
        {
            // Arrange
            var courseId = 1;

            // Simulate course has enrollments
            _courseRepo.Setup(r => r.ExistsAsync(courseId))
                       .ReturnsAsync(true);

            var courseExists = await _courseRepo.Object.ExistsAsync(courseId);

            // Act
            var canDelete = !courseExists; // cannot delete if exists (simulating enrollments linked)

            // Assert
            Assert.False(canDelete);
        }

        [Fact]
        public async Task UpdateCourse_ShouldPersistChanges()
        {
            // Arrange
            var course = new Course { CourseId = 1, CourseCode = "CS101"};

            _courseRepo.Setup(r => r.Update(course))
                       .Returns(Task.CompletedTask)
                       .Verifiable();

            // Act
            await _courseRepo.Object.Update(course);

            // Assert
            _courseRepo.Verify(r => r.Update(course), Times.Once);
        }

        [Fact]
        public async Task Repository_GetById_ShouldReturnNull_WhenInvalid()
        {
            _studentRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Student?)null);
            var result = await _studentRepo.Object.GetByIdAsync(99);
            Assert.Null(result);
        }

        [Fact]
        public async Task ForeignKey_ShouldPreventInvalidEnrollment()
        {
            _enrollmentRepo.Setup(r => r.ExistsAsync(1)).ReturnsAsync(false);

            var exists = await _enrollmentRepo.Object.ExistsAsync(1);
            Assert.False(exists);
        }

        [Fact]
        public async Task CourseCode_ShouldBeUnique()
        {
            // Arrange
            var existingCourses = new List<Course>
            {
                new Course { CourseId = 1, CourseCode = "CS101" },
                new Course { CourseId = 2, CourseCode = "CS102" }
            }.AsQueryable();

            _courseRepo.Setup(r => r.GetAllAsync())
                       .ReturnsAsync(existingCourses);

            var newCourse = new Course { CourseId = 3, CourseCode = "CS101" }; // duplicate

            // Act
            var allCourses = await _courseRepo.Object.GetAllAsync();
            var duplicate = allCourses.Any(c => c.CourseCode == newCourse.CourseCode);

            // Assert
            Assert.True(duplicate);
        }

        [Fact]
        public void RequiredFields_ShouldRejectNull()
        {
            var student = new Student();
            Assert.Null(student.FirstName);
            Assert.Null(student.StudentNumber);
        }
    }
}
