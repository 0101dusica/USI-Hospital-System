using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.UserManagement.Administrators.Model;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.UserManagement.Administrators.Repository
{
    public class AdministratorRepository : IAdministratorRepository
    {
        private static List<Administrator> _administrators = new List<Administrator>();
        private const string _storagePath = "../../../../Data/Administrator.json";

        private ISerializer<Administrator> _serializer;


        public AdministratorRepository(ISerializer<Administrator> serializer)
        {
            _serializer = serializer;
            _administrators = Load();
        }

        public List<Administrator> Load()
        {
            return _serializer.FromJson(_storagePath);
        }

        public void Save()
        {
            _serializer.ToJson(_storagePath, _administrators);
        }

        public List<Administrator> GetAll()
        {
            return _administrators;
        }

        public Administrator? GetByUsername(string username)
        {
            return _administrators.SingleOrDefault(a => a.Username.Equals(username));
        }


        public void Add(Administrator administrator)
        {
            _administrators.Add(administrator);
            Save();
        }

        public void Delete(Administrator administrator)
        {
            _administrators.Remove(administrator);
            Save();
        }

        public void Update(Administrator updatedAdministrator)
        {
            var existingAdministrator = GetByUsername(updatedAdministrator.Username);
            if (existingAdministrator != null)
            {
                existingAdministrator.FirstName = updatedAdministrator.FirstName;
                existingAdministrator.LastName = updatedAdministrator.LastName;
                existingAdministrator.Password = updatedAdministrator.Password;
                existingAdministrator.UserStatus = updatedAdministrator.UserStatus;

                Save();
            }
        }
    }
}
