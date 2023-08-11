using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Repository;
using ZdravoCorp.SchedulingManagement.FreeDays.Model;
using ZdravoCorp.SchedulingManagement.FreeDays.Repository;
using ZdravoCorp.SchedulingManagement.FreeDays.Service;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Doctors.Repository;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.SchedulingManagement.Appointments.Service
{
    public class AppointmentDoctorAvailabilityService
    {
        private readonly FreeDaysService _freeDaysService;
        private readonly DoctorService _doctorService;
        private readonly AppointmentService _appointmentService;

        public AppointmentDoctorAvailabilityService(DoctorService doctorService)
        {
            _freeDaysService = new FreeDaysService(new FreeDaysRepository(new Serializer<FreeDay>()));
            _doctorService = doctorService;
        }
        public AppointmentDoctorAvailabilityService()
        {
            _freeDaysService = new FreeDaysService(new FreeDaysRepository(new Serializer<FreeDay>()));
            _doctorService = new DoctorService(new DoctorRepository(new Serializer<Doctor>()));
            _appointmentService = new AppointmentService(new AppointmentRepository(new Serializer<Appointment>()));
        }

        public bool IsDoctorAvailabileForFreeDays(DateTime startDateTime, DateTime endDateTime, Doctor doctor)
        {
            foreach(string appointmentId in doctor.AppointmentIds)
            {
                Appointment? appointment = _appointmentService.GetById(appointmentId);
                if (_freeDaysService.IsAppointmentInRange(appointment, startDateTime, endDateTime))
                    return false;
            }
            return true;
        }
        public void CheckDoctorAvailability(DateTime startDateTime, DateTime endDateTime, Doctor doctor)
        {
            if (IsDoctorOccupiedDuringAppointments(startDateTime, endDateTime, doctor, _appointmentService) || IsDoctorHasFreeDays(startDateTime, endDateTime, doctor))
            {
                throw new Exception("Doctor is not available!");
            }
        }

        public bool isDoctorAvailable(DateTime startDateTime, DateTime endDateTime, Doctor doctor)
        {
            return !IsDoctorOccupiedDuringAppointments(startDateTime, endDateTime, doctor, _appointmentService) && !IsDoctorHasFreeDays(startDateTime, endDateTime, doctor);

        }

        public void CheckDoctorAvailability(DateTime startDateTime, DateTime endDateTime, Doctor doctor, AppointmentService appointmentService)
        {
            if (IsDoctorOccupiedDuringAppointments(startDateTime, endDateTime, doctor, appointmentService) || IsDoctorHasFreeDays(startDateTime, endDateTime, doctor))
            {
                throw new Exception("Doctor is not available!");
            }
        }

        private bool IsDoctorOccupiedDuringAppointments(DateTime startDateTime, DateTime endDateTime, Doctor doctor, AppointmentService appointmentService)
        {
            foreach (string appointmentId in doctor.AppointmentIds)
            {
                Appointment? appointment = appointmentService.GetById(appointmentId);
                if (appointment.AppointmentStatus == AppointmentStatus.Scheduled && IsTimeSlotOverlapping(startDateTime, endDateTime, appointment!.TimeSlot))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsDoctorHasFreeDays(DateTime startDateTime, DateTime endDateTime, Doctor doctor)
        {
            foreach (string freeDayId in doctor.FreeDaysIds)
            {
                FreeDay? freeDay = _freeDaysService.FindDoctorFreeDays(freeDayId);
                if (freeDay != null)
                {
                    if (IsTimeSlotOverlapping(startDateTime, endDateTime, freeDay.TimeSlot))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsTimeSlotOverlapping(DateTime startDateTime, DateTime endDateTime, TimeSlot timeSlot)
        {
            return startDateTime >= timeSlot.StartTime && startDateTime <= timeSlot.EndTime ||
                   endDateTime >= timeSlot.StartTime && endDateTime <= timeSlot.EndTime;
        }


        public void CheckIsDoctorUpdatedOrAvailable(Doctor doctor, Doctor newDoctor, Appointment appointment,
            DateTime startTimeAppointment, DateTime endTimeAppointment, AppointmentService appointmentService)
        {
            if (newDoctor.Username != doctor.Username)
            {
                CheckDoctorAvailability(startTimeAppointment, endTimeAppointment, newDoctor, appointmentService);
                _doctorService.RemoveAppointmentId(appointment.Id, doctor);
                doctor = newDoctor;
                _doctorService.AddAppointmentId(appointment.Id, doctor);
                return;

            }
            CheckDoctorAvailability(startTimeAppointment, endTimeAppointment, doctor, appointmentService);
        }

    }
}
