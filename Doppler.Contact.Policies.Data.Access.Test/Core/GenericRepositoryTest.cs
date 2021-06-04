using Doppler.Contact.Policies.Data.Access.Core;
using Doppler.Contact.Policies.Data.Access.Entities.Student;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Doppler.Contact.Policies.Data.Access.Test.Core
{
    public class GenericRepositoryTest
    {

        [Fact]
        public async void GetAllAsync__xxxxxxxx____should_return_all_entities()
        {

            var student = new List<Student>() {
            new Student(){ Id=1, Name="Juan"},
            new Student(){ Id=1, Name="Johan"},
            new Student(){ Id=1, Name="Lucia"},
            };
            // arrange
           




            var genericRepository = new GenericRepository<Student>(options);

            //act
            var allStudent = await genericRepository.GetAllAsync();

            //assert
            Assert.Equal(student.Count, allStudent.ToList().Count);


        }
    }
}
