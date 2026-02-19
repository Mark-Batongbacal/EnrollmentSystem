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
        private readonly Mock<IStudentRepository> _repo;
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

            _repo.Setup(r => r.AddAsync(It.IsAny<Student>()))
                 .Returns(Task.CompletedTask)
                 .Verifiable();

            // Act
            var result = await _service.CreateAsync(student);

            // Assert
            _repo.Verify(r => r.AddAsync(student), Times.Once);
            Assert.Equal(student, result);
        }

        [Fact]
        public async Task StudentNumber_ShouldBeUnique()
        {
            // Arrange
            var student1 = new Student { StudentId = 1, StudentNumber = 12345, FirstName = "Alice" };
            var student2 = new Student { StudentId = 2, StudentNumber = 12345, FirstName = "Bob" }; // duplicate

            // Mock repository: no student exists initially
            _repo.Setup(r => r.ExistsAsync(student1.StudentNumber))
                 .ReturnsAsync(false);

            _repo.Setup(r => r.AddAsync(student1))
                 .Returns(Task.CompletedTask)
                 .Verifiable();

            // Act: insert first student
            var result1 = await _service.CreateAsync(student1);

            // Assert first insert succeeded
            Assert.Equal(student1, result1);
            _repo.Verify(r => r.AddAsync(student1), Times.Once);

            // Setup repository to simulate duplicate
            _repo.Setup(r => r.ExistsAsync(student2.StudentNumber))
                 .ReturnsAsync(true);

            // Act & Assert: inserting second student throws exception
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await _service.CreateAsync(student2);
            });
        }
    }
}
