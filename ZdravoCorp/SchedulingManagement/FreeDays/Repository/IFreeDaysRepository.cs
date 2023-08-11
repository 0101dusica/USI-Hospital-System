using System.Collections.Generic;
using ZdravoCorp.SchedulingManagement.FreeDays.Model;

namespace ZdravoCorp.SchedulingManagement.FreeDays.Repository
{
    public interface IFreeDaysRepository
    {
        List<FreeDay> GetAll();
        FreeDay? GetById(string id);
        void Add(FreeDay freeDay);
        void Delete(FreeDay freeDay);
        void Update(FreeDay updatedFreeDay);
        string NextId();
        FreeDay? FindDoctorFreeDays(string freeDayId);
    }
}
