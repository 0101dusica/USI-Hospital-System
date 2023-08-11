using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.CommunicatonManagement.Polls.Model;
using ZdravoCorp.CommunicatonManagement.Polls.Service;
using ZdravoCorp.CommunicatonManagement.Polls.ViewModel;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Service;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.CommunicatonManagement.Polls.Command
{
    public class SubmitDoctorPollCommand : BaseCommand
    {
        private DoctorPollViewModel _patientDoctorPollViewModel;
        private DoctorService _doctorService;
        private PollService _pollService;
        private AppointmentService _appointmentService;
        private Patient _patient;

        public SubmitDoctorPollCommand(DoctorPollViewModel patientDoctorPollViewModel,
            AppointmentService appointmentService, PollService pollService, DoctorService doctorService, Patient patient)
        {
            _patientDoctorPollViewModel = patientDoctorPollViewModel;
            _doctorService = doctorService;
            _appointmentService = appointmentService;
            _pollService = pollService;
            _patient = patient;
         
        }
        public bool CanExecute(object? parameter)
        {
            if (_patientDoctorPollViewModel.Validate() && _patientDoctorPollViewModel.IsAppointmentSelected() &&
                _patientDoctorPollViewModel.CheckAppointmentStatus()) return true;
            return false;
        }
        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                Doctor? doctor = _doctorService.GetByUsername(_patientDoctorPollViewModel.SelectedAppointment!.DoctorUsername);
                List<int> newGrades = _patientDoctorPollViewModel.GetGrades();
                List<String> questions = _patientDoctorPollViewModel.GetQuestions();
                string comment = _patientDoctorPollViewModel.Comment!;
                Poll doctorPoll = new Poll(_pollService.NextId(), _patient.Username, PollType.Doctor, questions, comment, newGrades, doctor!.Username);
                _pollService.Add(doctorPoll);
                MessageBox.Show("Successfully doctor poll made!");
                _doctorService.UpdateDoctorGrades(doctor!, newGrades);
                _appointmentService.ChangeAppointmentRatedStatus(_patientDoctorPollViewModel.SelectedAppointment!);
        
            }
        }
    }
}
