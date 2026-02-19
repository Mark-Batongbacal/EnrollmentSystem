using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnrollmentSystem.Models.Database;
using EnrollmentSystem.Repository.Students;
using EnrollmentSystem.Services.Students;
using Moq;

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
        public async Task AddStudent_ShouldInsertRecord()
        {
            var student = new Student { StudentId = 1, FirstName = "Test Student", LastName = "Test", StudentNumber = 001 };
            _repo.Setup(r => r.AddAsync(student)).Returns(Task.CompletedTask).Verifiable();

            var result = await _service.CreateAsync(student);

            _repo.Verify(r => r.AddAsync(student), Times.Once);
            Assert.Equal(student, result);
        }

        [Fact]
        public async Task StudentNumber_ShouldBeUnique()
        {
            var student = new Student { StudentNumber = 001 };
            _repo.Setup(r => r.ExistsAsync(student.StudentNumber)).ReturnsAsync(true);

            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(student));
        }

        [Fact]
        public async Task RegisterStudent_ShouldPreventDuplicateStudentNumber()
        {
            var student = new Student { StudentNumber = 002 };
            _repo.Setup(r => r.ExistsAsync(student.StudentNumber)).ReturnsAsync(true);

            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(student));
        }
    }
}
