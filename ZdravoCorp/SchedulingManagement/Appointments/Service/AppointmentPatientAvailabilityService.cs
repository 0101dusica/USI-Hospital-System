using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.UserManagement.Patients.Model;

namespace ZdravoCorp.SchedulingManagement.Appointments.Service
{
    public class AppointmentPatientAvailabilityService
    {
        public AppointmentPatientAvailabilityService()
        {

        }

        public bool IsPatientAvailable(Appointment newAppointment, Patient patient, AppointmentService appointmentService)
        {
            foreach (string id in patient.MedicalRecord.AppointmentIds)
            {
                Appointment? appointment = appointmentService.GetById(id);
                if (appointment.AppointmentStatus == AppointmentStatus.Scheduled && newAppointment.TimeSlot.StartTime >= appointment!.TimeSlot.StartTime && newAppointment.TimeSlot.StartTime <= appointment.TimeSlot.EndTime
                    || newAppointment.TimeSlot.EndTime >= appointment.TimeSlot.StartTime && newAppointment.TimeSlot.EndTime <= appointment.TimeSlot.EndTime)
                {
                    throw new Exception("Patient is not available!"); ;
                }
            }
            return true;
        }
    }
}
