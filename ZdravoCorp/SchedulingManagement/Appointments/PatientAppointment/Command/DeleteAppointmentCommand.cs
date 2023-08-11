using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.Appointments.PatientAppointment.ViewModel;
using ZdravoCorp.SchedulingManagement.Appointments.Service;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.SchedulingManagement.Appointments.PatientAppointment.Command
{
    public class DeleteAppointmentCommand : BaseCommand
    {
        private AppointmentService _appointmentService;
        private PatientAppointmentActionsService _patientAppointmentActionsService;
        private PatientAppointmentsViewModel _patientAppointmentsViewModel;
        public DeleteAppointmentCommand(PatientAppointmentsViewModel patientAppointmentsViewModel, AppointmentService appointmentService, PatientAppointmentActionsService patientAppointmentActionsService)
        {
            _patientAppointmentsViewModel = patientAppointmentsViewModel;
            _appointmentService = appointmentService;
            _patientAppointmentActionsService = patientAppointmentActionsService;

        }
        public override void Execute(object? parameter)
        {
            Appointment? appointment = _patientAppointmentsViewModel.SelectedAppointment;
            if (appointment != null)
            {
                _appointmentService.CancelAppointment(appointment);
                _patientAppointmentsViewModel.SelectedAppointment = null;
                _patientAppointmentsViewModel.LoadDataGrid(_appointmentService);
                _patientAppointmentActionsService.AddPatientAppointmentAction(PatientAction.Delete);
                Dictionary<string, int> actions = _patientAppointmentActionsService.LoadPatientActions();
                bool needBlock = _patientAppointmentActionsService.CheckPatientActions(actions);
                if (needBlock)
                {
                    _patientAppointmentActionsService.BlockAccount();
                }
            }
            else
            {
                MessageBox.Show("Row must be selected!");
            }
        }
    }
}
