using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.FreeDays.Model;
using ZdravoCorp.SchedulingManagement.FreeDays.Repository;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Doctors.Repository;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.SchedulingManagement.FreeDays.Service
{
    public class FreeDaysService
    {
        private IFreeDaysRepository _freeDaysRepository;

        private DoctorService _doctorService;
        public FreeDaysService(IFreeDaysRepository freeDaysRepository)
        {
            _freeDaysRepository = freeDaysRepository;
            _doctorService = new DoctorService(new DoctorRepository(new Serializer<Doctor>()));
        }

        public List<FreeDay> GetAll()
        {
            return _freeDaysRepository.GetAll();
        }

        public FreeDay? GetById(string id)
        {
            return _freeDaysRepository.GetById(id);
        }

        public void Add(FreeDay freeDay)
        {
            _freeDaysRepository.Add(freeDay);
        }

        public void Delete(FreeDay freeDay)
        {
            _freeDaysRepository.Delete(freeDay);
        }

        public void Update(FreeDay freeDay)
        {
            _freeDaysRepository.Update(freeDay);
        }

        public string NextId()
        {
            return _freeDaysRepository.NextId();
        }

        public FreeDay? FindDoctorFreeDays(string freeDayId)
        {
            return _freeDaysRepository.FindDoctorFreeDays(freeDayId);
        }

        public void CreateFreeDays(FreeDay freeDay)
        {
            Doctor? doctor = _doctorService.GetByUsername(freeDay.DoctorUsername);
            Add(freeDay);
            _doctorService.AddFreeDaysId(freeDay.Id, doctor);
        }

        public bool IsAppointmentInRange(Appointment? appointment, DateTime startDateTime, DateTime endDateTime)
        {
             return ((appointment!.TimeSlot.StartTime >= startDateTime && appointment!.TimeSlot.StartTime <= endDateTime) || (appointment!.TimeSlot.EndTime >= startDateTime && appointment!.TimeSlot.EndTime <= endDateTime));
        }
    }
}
