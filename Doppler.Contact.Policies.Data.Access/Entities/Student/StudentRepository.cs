using Doppler.Contact.Policies.Data.Access.Core;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doppler.Contact.Policies.Data.Access.Entities.Student
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {

        public StudentRepository(IDatabaseConnectionFactory dataBaseConnectionFactory) : base(dataBaseConnectionFactory)
        {
            TableName = "Student";
        }



    }
}
