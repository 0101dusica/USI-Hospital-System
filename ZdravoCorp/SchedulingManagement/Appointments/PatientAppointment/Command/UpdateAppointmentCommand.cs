using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.SchedulingManagement.Appointments.PatientAppointment.ViewModel;
using ZdravoCorp.SchedulingManagement.Appointments.Service;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.SchedulingManagement.Appointments.PatientAppointment.Command
{
    public class UpdateAppointmentCommand : BaseCommand
    {
        private AppointmentService _appointmentService;
        private DoctorService _doctorService;
        private PatientService _patientService;
        private PatientAppointmentsViewModel _viewModel;

        public UpdateAppointmentCommand(PatientAppointmentsViewModel viewModel, AppointmentService appointmentService, DoctorService doctorService, PatientService patientService)
        {
            _viewModel = viewModel;
            _appointmentService = appointmentService;
            _doctorService = doctorService;
            _patientService = patientService;

        }

        public override void Execute(object? parameter)
        {
            try
            {
                if (_viewModel.SelectedAppointment != null)
                {
                    if (_viewModel.ValidateInputs() && _viewModel.CheckIsDate24HDiff(_viewModel.SelectedAppointment!))
                    {
                        DateTime startDateTime =
                            _viewModel.GetAppointmentDateTime(_viewModel.DateInputText, _viewModel.TimeInputText);
                        DateTime endDateTime = startDateTime.AddMinutes(15);
                        Doctor selectedDoctor = _viewModel.SelectedDoctor!;
                        Doctor? doctor = _doctorService.GetByUsername(_viewModel.SelectedAppointment!.DoctorUsername);
                        _appointmentService.TryUpdate(_viewModel.SelectedAppointment!, doctor!, selectedDoctor,
                            startDateTime, endDateTime, _viewModel.Patient);
                        _viewModel.LoadDataGrid(_viewModel.AppointmentService);
                        _viewModel.PatientAppointmentActionsService.AddPatientAppointmentAction(PatientAction.Update);
                        MessageBox.Show("Appointment is updated!");
                        Dictionary<string, int> actions = _viewModel.PatientAppointmentActionsService.LoadPatientActions();
                        bool needBlock = _viewModel.PatientAppointmentActionsService.CheckPatientActions(actions);
                        if (needBlock)
                        {
                            _viewModel.PatientAppointmentActionsService.BlockAccount();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Row must be selected!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
