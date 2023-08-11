using System.Collections.Generic;
using ZdravoCorp.UserManagement.Administrators.Model;

namespace ZdravoCorp.UserManagement.Administrators.Repository
{
    public interface IAdministratorRepository
    {
        List<Administrator> GetAll();
        Administrator? GetByUsername(string username);
        void Add(Administrator administrator);
        void Delete(Administrator administrator);
        void Update(Administrator updatedAdministrator);
    }
}
