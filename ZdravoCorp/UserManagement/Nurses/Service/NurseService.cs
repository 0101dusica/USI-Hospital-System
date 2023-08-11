using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.UserManagement.Nurses.Model;
using ZdravoCorp.UserManagement.Nurses.Repository;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.UserManagement.Nurses.Service
{
    public class NurseService
    {
        private INurseRepository _nurseRepository;

        public NurseService(INurseRepository nurseRepository)
        {
            _nurseRepository = nurseRepository;
        }

        public List<Nurse> GetAll()
        {
            return _nurseRepository.GetAll();
        }

        public Nurse? GetByUsername(string username)
        {
            return _nurseRepository.GetByUsername(username);
        }

        public void Add(Nurse nurse)
        {
            _nurseRepository.Add(nurse);
        }


        public void Delete(Nurse nurse)
        {
            _nurseRepository.Delete(nurse);
        }

        public void Update(Nurse updatedNurse)
        {
            _nurseRepository.Update(updatedNurse);
        }

        public ObservableCollection<Nurse> GetAllNursesExceptLoggedIn(Nurse nurse)
        {
            return _nurseRepository.GetAllNursesExceptLoggedIn(nurse);
        }
    }
}
