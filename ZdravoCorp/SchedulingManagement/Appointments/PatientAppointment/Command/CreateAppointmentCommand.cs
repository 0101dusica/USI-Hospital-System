using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.Appointments.PatientAppointment.ViewModel;
using ZdravoCorp.SchedulingManagement.Appointments.Service;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.SchedulingManagement.Appointments.PatientAppointment.Command
{
    public class CreateAppointmentCommand : BaseCommand
    {
        private AppointmentService _appointmentService;
        private DoctorService _doctorService;
        private PatientService _patientService;
        private PatientAppointmentsViewModel _viewModel;

        public CreateAppointmentCommand(PatientAppointmentsViewModel viewModel, AppointmentService appointmentService, DoctorService doctorService, PatientService patientService)
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
                if (_viewModel.ValidateInputs())
                {
                    var startDateTime = _viewModel.GetAppointmentDateTime(_viewModel.DateInputText, _viewModel.TimeInputText);
                    var endDateTime = startDateTime.AddMinutes(15);
                    var appointment = new Appointment(_appointmentService.NextId(), _viewModel.SelectedDoctor!.Username, _viewModel.Patient.Username, AppointmentType.Appointment, AppointmentStatus.Scheduled, "", new Anamnesis(), new TimeSlot(startDateTime, endDateTime));
                    _appointmentService.CreateAppointment(_doctorService, _patientService, appointment);
                    MessageBox.Show("Sucessfully Added Appointment!");
                    _viewModel.PatientAppointmentActionsService.AddPatientAppointmentAction(PatientAction.Create);
                    _viewModel.LoadDataGrid(_appointmentService);
                    Dictionary<string, int> actions = _viewModel.PatientAppointmentActionsService.LoadPatientActions();
                    bool needBlock = _viewModel.PatientAppointmentActionsService.CheckPatientActions(actions);
                    if (needBlock)
                    {
                        _viewModel.PatientAppointmentActionsService.BlockAccount();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
