using EnrollmentSystem.Models.Database;
using EnrollmentSystem.Services.Students;
using Moq;
using Xunit;
using System.Threading.Tasks;
using EnrollmentSystem.Repository.Students;

namespace EnrollmentSystem.Test
{
    public class StudentServiceTest
    {
        private readonly Mock<IStudentRepository> _repo = new();
        private readonly StudentService _service;

        public StudentServiceTest()
        {
            _repo = new Mock<IStudentRepository>();
            _service = new StudentService(_repo.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldCallRepository()
        {
            // Arrange
            var student = new Student { StudentId = 1, FirstName = "Test Student" };

            _repo.Setup(r => r.AddAsync(student))
                 .Returns(Task.CompletedTask)
                 .Verifiable();

            // Act
            var result = await _service.CreateAsync(student);

            // Assert
            _repo.Verify(r => r.AddAsync(student), Times.Once);
            Assert.Equal(student, result);
        }
    }
}
