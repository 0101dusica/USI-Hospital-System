using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.UserManagement
{
    public enum UserStatus
    {
        Active,
        Blocked

    }

    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public UserStatus UserStatus { get; set; }

        public User() { }

        public User(string firstName, string lastName, string username, string password, UserStatus userStatus)
        {
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Password = password;
            UserStatus = userStatus;
        }
    }
}
