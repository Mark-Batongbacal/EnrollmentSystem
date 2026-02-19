using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnrollmentSystem.Models.Database;
using EnrollmentSystem.Repository.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using Xunit;

namespace EnrollmentSystemTest
{
    [TestClass]
    public class StudentRepositoryTest
    {
        private static DbContextOptions<EnrollmentSystemContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<EnrollmentSystemContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task Integration_Add_Update_Delete_Exists_Works()
        {
            var options = CreateNewContextOptions();

            var student = new Student
            {
                StudentId = 1,
                StudentNumber = 1001,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com"
            };

            // Add
            using (var context = new EnrollmentSystemContext(options))
            {
                var repo = new StudentRepository(context);
                await repo.AddAsync(student);
            }

            // Verify Add/Get/Exists/GetAll
            using (var context = new EnrollmentSystemContext(options))
            {
                var repo = new StudentRepository(context);

                Assert.True(await repo.ExistsAsync(1));

                var fetched = await repo.GetByIdAsync(1);
                Assert.NotNull(fetched);
                Assert.Equal("John", fetched!.FirstName);
                Assert.Equal("Doe", fetched.LastName);

                var all = (await repo.GetAllAsync()).ToList();
                Assert.Single(all);
            }

            // Update
            using (var context = new EnrollmentSystemContext(options))
            {
                var repo = new StudentRepository(context);
                var toUpdate = await repo.GetByIdAsync(1);
                Assert.IsNotNull(toUpdate);
                toUpdate!.FirstName = "Jane";
                await repo.Update(toUpdate);
            }

            using (var context = new EnrollmentSystemContext(options))
            {
                var repo = new StudentRepository(context);
                var fetched = await repo.GetByIdAsync(1);
                Assert.IsNotNull(fetched);
                Assert.AreEqual("Jane", fetched!.FirstName);
            }

            // Delete
            using (var context = new EnrollmentSystemContext(options))
            {
                var repo = new StudentRepository(context);
                var toDelete = await repo.GetByIdAsync(1);
                Assert.IsNotNull(toDelete);
                await repo.Delete(toDelete!);
            }

            using (var context = new EnrollmentSystemContext(options))
            {
                var repo = new StudentRepository(context);
                Assert.IsFalse(await repo.ExistsAsync(1), "ExistsAsync should return false after Delete.");
            }
        }
    }
}