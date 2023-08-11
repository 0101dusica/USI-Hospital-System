using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.UserManagement.Administrators.Model
{
    public class Administrator : User
    {
        public Administrator()
        {

        }
        public Administrator(User person)
        {
            FirstName = person.FirstName;
            LastName = person.LastName;
            Username = person.Username;
            Password = person.Password;
            UserStatus = person.UserStatus;
        }


    }
}
