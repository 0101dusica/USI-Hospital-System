using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.SchedulingManagement.Appointments.Repository
{
    public class PatientAppointmentActionsRepository : IPatientAppointmentActionsRepository
    {
        private static List<PatientAppointmentActions> _patientAppointmentActions = new List<PatientAppointmentActions>();
        private const string _storagePath = "../../../Data/PatientAppointmentActions.json";

        private ISerializer<PatientAppointmentActions> _serializer;


        public PatientAppointmentActionsRepository(ISerializer<PatientAppointmentActions> serializer)
        {
            _serializer = serializer;
            _patientAppointmentActions = Load();
        }

        public List<PatientAppointmentActions> Load()
        {
            return _serializer.FromJson(_storagePath);
        }

        public void Save()
        {
            _serializer.ToJson(_storagePath, _patientAppointmentActions);
        }

        public List<PatientAppointmentActions> GetAll()
        {
            return _patientAppointmentActions;
        }

        public void Add(PatientAppointmentActions action)
        {
            _patientAppointmentActions.Add(action);
            Save();
        }

        public void Delete(PatientAppointmentActions action)
        {
            _patientAppointmentActions.Remove(action);
            Save();
        }

    }
}
