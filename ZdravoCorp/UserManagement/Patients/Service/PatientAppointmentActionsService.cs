using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.SchedulingManagement.Appointments.Repository;
using ZdravoCorp.UserManagement.Patients.Model;

namespace ZdravoCorp.UserManagement.Patients.Service
{
    public class PatientAppointmentActionsService
    {
        private IPatientAppointmentActionsRepository _patientAppointmentActionsRepository;
        private Patient _loggedPatient;

        public PatientAppointmentActionsService(Patient patient, IPatientAppointmentActionsRepository patientAppointmentActionsRepository)
        {
            _patientAppointmentActionsRepository = patientAppointmentActionsRepository;
            _loggedPatient = patient;
        }

        public List<PatientAppointmentActions> GetAll()
        {
            return _patientAppointmentActionsRepository.GetAll();
        }

        public void Add(PatientAppointmentActions action)
        {
            _patientAppointmentActionsRepository.Add(action);
        }

        public void Delete(PatientAppointmentActions action)
        {
            _patientAppointmentActionsRepository.Delete(action);
        }

        public void AddPatientAppointmentAction(PatientAction action)
        {
            PatientAppointmentActions patientAppointmentActions = new PatientAppointmentActions(
                _loggedPatient.Username, action, DateTime.Now);
            _patientAppointmentActionsRepository.Add(patientAppointmentActions);
        }

        public Dictionary<string, int> LoadPatientActions()
        {
            DateTime thirtyDaysAgo = DateTime.Now.AddDays(-30);
            Dictionary<string, int> actions = InitializeActionCounts();

            foreach (PatientAppointmentActions action in _patientAppointmentActionsRepository.GetAll())
            {
                if (IsWithinTimeRange(action, thirtyDaysAgo, DateTime.Now))
                {
                    UpdateActionCount(actions, action.PatientAction);
                }
            }
            return actions;
        }

        private Dictionary<string, int> InitializeActionCounts()
        {
            return new Dictionary<string, int>
            {
                ["Create"] = 0,
                ["Update"] = 0,
                ["Delete"] = 0
            };
        }

        private bool IsWithinTimeRange(PatientAppointmentActions action, DateTime startDate, DateTime endDate)
        {
            return _loggedPatient.Username == action.PatientUsername && action.TimeAction > startDate &&
                   action.TimeAction <= endDate;
        }

        private void UpdateActionCount(Dictionary<string, int> actions, PatientAction patientAction)
        {
            if (patientAction == PatientAction.Create)
            {
                actions["Create"]++;
            }
            else if (patientAction == PatientAction.Delete)
            {
                actions["Delete"]++;
            }
            else if (patientAction == PatientAction.Update)
            {
                actions["Update"]++;
            }
        }
        public bool CheckPatientActions(Dictionary<string, int> actions)
        {
            const int maxCreateActions = 8;
            const int maxEditActions = 5;
            const int maxDeleteActions = 5;

            return actions["Create"] >= maxCreateActions || actions["Update"] >= maxEditActions || actions["Delete"] >= maxDeleteActions;
        }

        public void BlockAccount()
        {
            _loggedPatient.UserStatus = UserStatus.Blocked;
            MessageBox.Show("Your account is blocked!");
        }
    }
}

