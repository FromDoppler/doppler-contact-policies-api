using Doppler.Contact.Policies.Data.Access.Core;
using Doppler.Contact.Policies.Data.Access.Entities.Student;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using ServiceStack;
using Xunit;

namespace Doppler.Contact.Policies.Data.Access.Test.Core
{
    public class GenericRepositoryTest
    {
        private InMemoryDatabase _inMemoryDataBase;

        #region Test methods

        [Fact]
        public async Task GetAll_Should_ReturnAllStudents()
        {
            // Arrange
            CreateDataBase();
            var expected = InsertStudents();
            var connectionFactoryMock = new Mock<IDatabaseConnectionFactory>();
            connectionFactoryMock.Setup(c => c.GetConnection().Result).Returns(_inMemoryDataBase.OpenConnection());

            // Act
            var result = await new StudentRepository(connectionFactoryMock.Object).GetAllAsync();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveSameCount(expected);
        }

        [Theory]
        [InlineData(1)]
        public async Task Get_Should_ReturnStudentWithId(long id)
        {
            // Arrange
            CreateDataBase();
            var students = InsertStudents();
            var connectionFactoryMock = new Mock<IDatabaseConnectionFactory>();
            connectionFactoryMock.Setup(c => c.GetConnection().Result)
                .Returns(_inMemoryDataBase.OpenConnection());

            // Act
            var result = await new StudentRepository(connectionFactoryMock.Object).GetAsync(id);
            var expected = students.SingleOrDefault(x => x.Id == id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(expected.Id);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(5)]
        public async Task Get_Should_ReturnNull_When_Id_is_Not_Found(long id)
        {
            // Arrange
            CreateDataBase();
            InsertStudents();
            var connectionFactoryMock = new Mock<IDatabaseConnectionFactory>();
            connectionFactoryMock.Setup(c => c.GetConnection().Result).Returns(_inMemoryDataBase.OpenConnection());

            // Act
            var result = await new StudentRepository(connectionFactoryMock.Object).GetAsync(id);

            // Assert
            result.Should().BeNull("Id could not be found");
        }

        [Fact]
        public async Task Insert_Should_ReturnAffectedRows_When_StudentIsInserted()
        {
            // Arrange
            CreateDataBase();
            var studentToInsert = new Student() {Id = 1, Name = "Juan"};
            var connectionFactoryMock = new Mock<IDatabaseConnectionFactory>();
            connectionFactoryMock.Setup(c => c.GetConnection().Result).Returns(_inMemoryDataBase.OpenConnection());

            // Act
            var result = await new StudentRepository(connectionFactoryMock.Object).InsertAsync(studentToInsert);
//            var student = _inMemoryDataBase.GetId(studentToInsert);

            // Assert
            result.Should().Be(1);
        }

        #endregion

        #region private methods

        private void CreateDataBase()
        {
            _inMemoryDataBase = new InMemoryDatabase();
        }
        private List<Student> InsertStudents()
        {
            var students = new List<Student>
            {
                new Student() {Id = 1, Name = "Juan"},
                new Student {Id = 2, Name = "Pedro"},
                new Student {Id = 3, Name = "Pablo"},
            };
            _inMemoryDataBase.Insert(students);
            return students;
        }

        #endregion
    }
}
