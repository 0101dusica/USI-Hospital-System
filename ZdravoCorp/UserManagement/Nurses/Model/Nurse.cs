using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.UserManagement.Nurses.Model
{
    public class Nurse : User
    {
        public Nurse() { }
        public Nurse(User person)
        {
            FirstName = person.FirstName;
            LastName = person.LastName;
            Username = person.Username;
            Password = person.Password;
            UserStatus = person.UserStatus;
        }

    }
}
