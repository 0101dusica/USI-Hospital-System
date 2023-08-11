using System.Collections.Generic;
using System.Collections.ObjectModel;
using ZdravoCorp.UserManagement.Nurses.Model;

namespace ZdravoCorp.UserManagement.Nurses.Repository
{
    public interface INurseRepository
    {
        List<Nurse> GetAll();
        Nurse? GetByUsername(string username);
        void Add(Nurse nurse);
        void Delete(Nurse nurse);
        void Update(Nurse updatedNurse);
        ObservableCollection<Nurse> GetAllNursesExceptLoggedIn(Nurse nurse);
    }
}
