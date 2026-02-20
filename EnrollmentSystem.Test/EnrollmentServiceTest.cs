using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EnrollmentSystem.Models.Database;
using EnrollmentSystem.Repository.CourseOfferings;
using EnrollmentSystem.Repository.Enrollments;
using EnrollmentSystem.Repository.Logs;
using EnrollmentSystem.Services.Enrollments;
using Moq;
using Xunit;

namespace EnrollmentSystem.Test
{
    public class EnrollmentServiceTests
    {
        private readonly Mock<IEnrollmentRepository> _enrollmentRepo;
        private readonly Mock<ICourseOfferingRepository> _courseOfferingRepo;
        private readonly Mock<EnrollmentSystemContext> _contextMock;
        private readonly Mock<ILogRepository> _logRepo;
        private readonly EnrollmentService _service;

        public EnrollmentServiceTests()
        {
            _enrollmentRepo = new Mock<IEnrollmentRepository>();
            _courseOfferingRepo = new Mock<ICourseOfferingRepository>();
            _contextMock = new Mock<EnrollmentSystemContext>();
            _logRepo = new Mock<ILogRepository>();

            _service = new EnrollmentService( _courseOfferingRepo.Object, _enrollmentRepo.Object, _logRepo.Object, _contextMock.Object);
        }


        [Fact]
        public async Task Enrollment_ShouldBeUnique_PerStudentOffering()
        {
            int studentId = 1;
            int courseOfferingId = 1;

            _enrollmentRepo.Setup(r => r.AnyByStudentIdAsync(studentId))
                           .ReturnsAsync(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.EnrollAsync(studentId, courseOfferingId));
        }

        [Fact]
        public async Task EnrollStudent_ShouldFail_WhenCourseIsFull()
        {
            var enrollment = new Enrollment { StudentId = 1, CourseOfferingId = 1 };
            _enrollmentRepo.Setup(r => r.AnyByCourseOfferingIdAsync(enrollment.CourseOfferingId))
                .ReturnsAsync(true); 

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.EnrollAsync(enrollment.StudentId, enrollment.CourseOfferingId));
        }

        [Fact]
        public async Task EnrollStudent_ShouldFail_WhenAlreadyEnrolled()
        {
            var enrollment = new Enrollment { StudentId = 1, CourseOfferingId = 1 };
            _enrollmentRepo.Setup(r => r.AnyByStudentIdAsync(enrollment.StudentId))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.EnrollAsync(enrollment.StudentId, enrollment.CourseOfferingId));
        }

        [Fact]
        public async Task EnrollStudent_ShouldSucceed_WhenValid()
        {
            var enrollment = new Enrollment { StudentId = 1, CourseOfferingId = 1 };

            _enrollmentRepo.Setup(r => r.AnyByStudentIdAsync(enrollment.StudentId))
                           .ReturnsAsync(false);

            _enrollmentRepo.Setup(r => r.AnyByCourseOfferingIdAsync(enrollment.CourseOfferingId))
                           .ReturnsAsync(false);

            _courseOfferingRepo.Setup(r => r.IsWithinSemesterAsync(enrollment.CourseOfferingId, It.IsAny<DateOnly>()))
                               .ReturnsAsync(true);

            _enrollmentRepo.Setup(r => r.AddAsync(It.IsAny<Enrollment>()))
                           .Returns(Task.CompletedTask)
                           .Verifiable();

            var result = await _service.EnrollAsync(enrollment.StudentId, enrollment.CourseOfferingId);

            _enrollmentRepo.Verify(r => r.AddAsync(It.IsAny<Enrollment>()), Times.Once);
            Assert.Equal(enrollment.StudentId, result.StudentId);
            Assert.Equal(enrollment.CourseOfferingId, result.CourseOfferingId);
        }


        [Fact]
        public async Task EnrollStudent_ShouldFail_WhenSemesterExpired()
        {
            var enrollment = new Enrollment { StudentId = 1, CourseOfferingId = 1 };
            _enrollmentRepo.Setup(r => r.AnyByStudentIdAsync(enrollment.StudentId)).ReturnsAsync(false);
            _enrollmentRepo.Setup(r => r.AnyByCourseOfferingIdAsync(enrollment.CourseOfferingId)).ReturnsAsync(false);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.EnrollAsync(enrollment.StudentId, enrollment.CourseOfferingId));
        }

        [Fact]
        public async Task AssignGrade_ShouldFail_WhenNotEnrolled()
        {
            _enrollmentRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Enrollment?)null);

            var result = await _service.UpdateGradeAsync(1, 90);
            Assert.False(result);
        }

        [Fact]
        public async Task AssignGrade_ShouldFail_WhenGradeOutOfRange()
        {
            var enrollment = new Enrollment { EnrollmentId = 1, StudentId = 1 };
            _enrollmentRepo.Setup(r => r.GetByIdAsync(enrollment.EnrollmentId)).ReturnsAsync(enrollment);

            await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateGradeAsync(enrollment.EnrollmentId, 150));
        }

        [Fact]
        public async Task AssignGrade_ShouldSucceed_WhenValid()
        {
            var enrollment = new Enrollment { EnrollmentId = 1, StudentId = 1 };
            _enrollmentRepo.Setup(r => r.GetByIdAsync(enrollment.EnrollmentId)).ReturnsAsync(enrollment);
            _enrollmentRepo.Setup(r => r.UpdateAsync(enrollment)).Returns(Task.CompletedTask).Verifiable();

            var result = await _service.UpdateGradeAsync(enrollment.EnrollmentId, 95);

            _enrollmentRepo.Verify(r => r.UpdateAsync(enrollment), Times.Once);
            Assert.True(result);
            Assert.Equal(95, enrollment.FinalGrade);
        }
        

        [Fact]
        public async Task DeleteCourse_ShouldReturnFalse_WhenEnrollmentsExist()
        {
            int courseOfferingId = 1;

            _enrollmentRepo.Setup(r => r.AnyByCourseOfferingIdAsync(courseOfferingId))
                           .ReturnsAsync(true); 

            var canDelete = !await _enrollmentRepo.Object.AnyByCourseOfferingIdAsync(courseOfferingId);

            Assert.False(canDelete);
        }
    }
}
