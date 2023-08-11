using System.Collections.Generic;
using ZdravoCorp.UserManagement.Patients.Model;

namespace ZdravoCorp.SchedulingManagement.Appointments.Repository
{
    public interface IPatientAppointmentActionsRepository
    {
        List<PatientAppointmentActions> GetAll();
        void Add(PatientAppointmentActions action);
        void Delete(PatientAppointmentActions action);
    }
}
