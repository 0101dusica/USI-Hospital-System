using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.UserManagement.Administrators.Model;
using ZdravoCorp.UserManagement.Administrators.Repository;

namespace ZdravoCorp.UserManagement.Administrators.Service
{
    public class AdministratorService
    {
        private IAdministratorRepository _administratorRepository;

        public AdministratorService(IAdministratorRepository administratorRepository)
        {
            _administratorRepository = administratorRepository;
        }

        public List<Administrator> GetAll()
        {
            return _administratorRepository.GetAll();
        }

        public Administrator? GetByUsername(string username)
        {
            return _administratorRepository.GetByUsername(username);
        }

        public void Add(Administrator administrator)
        {
            _administratorRepository.Add(administrator);
        }

        public void Delete(Administrator administrator)
        {
            _administratorRepository.Delete(administrator);
        }

        public void Update(Administrator updatedAdministrator)
        {
            _administratorRepository.Update(updatedAdministrator);
        }
    }
}

