using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.UserManagement.Nurses.Model;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.UserManagement.Nurses.Repository
{
    public class NurseRepository : INurseRepository
    {
        private static List<Nurse> _nurses = new List<Nurse>();
        private const string _storagePath = "../../../../Data/Nurses.json";

        private ISerializer<Nurse> _serializer;


        public NurseRepository(ISerializer<Nurse> serializer)
        {
            _serializer = serializer;
            _nurses = Load();
        }

        public List<Nurse> Load()
        {
            return _serializer.FromJson(_storagePath);
        }

        public void Save()
        {
            _serializer.ToJson(_storagePath, _nurses);
        }

        public List<Nurse> GetAll()
        {
            return _nurses;
        }

        public Nurse? GetByUsername(string username)
        {
            return _nurses.FirstOrDefault(n => n.Username.Equals(username));
        }

        public void Add(Nurse nurse)
        {
            _nurses.Add(nurse);
            Save();
        }

        public void Delete(Nurse nurse)
        {
            _nurses.Remove(nurse);
            Save();
        }

        public void Update(Nurse updatedNurse)
        {
            var existingNurse = GetByUsername(updatedNurse.Username);
            if (existingNurse != null)
            {
                existingNurse.FirstName = updatedNurse.FirstName;
                existingNurse.LastName = updatedNurse.LastName;
                existingNurse.Password = updatedNurse.Password;
                existingNurse.UserStatus = updatedNurse.UserStatus;

                Save();
            }
        }

        public ObservableCollection<Nurse> GetAllNursesExceptLoggedIn(Nurse loggedNurse)
        {
            ObservableCollection<Nurse> nurses = new ObservableCollection<Nurse>();
            foreach (var nurse in _nurses)
            {
                if (!(nurse.Username.Equals(loggedNurse.Username))) nurses.Add(nurse);
            }

            return nurses;
        }
    }
}
