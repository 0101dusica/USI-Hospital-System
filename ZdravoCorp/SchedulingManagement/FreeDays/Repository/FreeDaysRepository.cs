using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.SchedulingManagement.FreeDays.Model;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.SchedulingManagement.FreeDays.Repository
{
    public class FreeDaysRepository : IFreeDaysRepository
    {
        private static List<FreeDay> _freeDays = new List<FreeDay>();
        private const string _storagePath = "../../../../Data/FreeDays.json";

        private ISerializer<FreeDay> _serializer;


        public FreeDaysRepository(ISerializer<FreeDay> serializer)
        {
            _serializer = serializer;
            _freeDays = Load();
        }

        public List<FreeDay> Load()
        {
            return _serializer.FromJson(_storagePath);
        }

        public void Save()
        {
            _serializer.ToJson(_storagePath, _freeDays);
        }

        public List<FreeDay> GetAll()
        {
            return _freeDays;
        }

        public FreeDay? GetById(string id)
        {
            return _freeDays.FirstOrDefault(f => f.Id.Equals(id));
        }

        public void Add(FreeDay freeDay)
        {
            _freeDays.Add(freeDay);
            Save();
        }



        public void Delete(FreeDay freeDay)
        {
            _freeDays.Remove(freeDay);
            Save();
        }

        public void Update(FreeDay updatedFreeDay)
        {
            var existingFreeDay = GetById(updatedFreeDay.Id);
            if (existingFreeDay != null)
            {
                existingFreeDay.RequestStatus = updatedFreeDay.RequestStatus;
                existingFreeDay.Description = updatedFreeDay.Description;
                existingFreeDay.TimeSlot = updatedFreeDay.TimeSlot;

                Save();
            }
        }

        public string NextId()
        {
            string? lastId = _freeDays.LastOrDefault()?.Id;
            if (lastId == null)
            {
                return "freeDays1";
            }
            else
            {
                int lastIdNumber = int.Parse(lastId.Replace("freeDays", ""));
                return $"freeDays{lastIdNumber + 1}";
            }
        }

        public FreeDay? FindDoctorFreeDays(string freeDayId)
        {
            return GetAll().FirstOrDefault(freeDays => freeDays.Id == freeDayId);
        }
    }
}
