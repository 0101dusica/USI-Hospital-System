using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.CommunicatonManagement.Polls.Service;
using ZdravoCorp.CommunicatonManagement.Polls.View;
using ZdravoCorp.CommunicatonManagement.Polls.ViewModel;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.ViewModel;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Service;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Command
{
    public class ShowDoctorPollCommand : BaseCommand
    {
        private PatientMedicalRecordViewModel _patientMedicalRecordViewModel;
        private AppointmentService _appointmentService;
        private PollService _pollService;
        private DoctorService _doctorService;
        private Patient _patient;

        public ShowDoctorPollCommand(PatientMedicalRecordViewModel patientMedicalRecordViewModel,
            AppointmentService appointmentService, PollService pollService, DoctorService doctorService, Patient patient)
        {
            _patientMedicalRecordViewModel = patientMedicalRecordViewModel;
            _appointmentService = appointmentService;
            _pollService = pollService;
            _doctorService = doctorService;
            _patient = patient;
            

        }

        public bool CanExecute(object? parameter)
        {
            if (_patientMedicalRecordViewModel.SelectedAppointment == null)
            {
                MessageBox.Show("You must select row first!");
                return false;
            }

            if (_patientMedicalRecordViewModel.SelectedAppointment.RatedStatus.Equals(RatedStatus.Rated))
            {
                MessageBox.Show("Doctor for this appointment is already rated!");
                return false;
            }

            return true;
        }

        public override void Execute(object? parameter)
        {
            if (!CanExecute(parameter)) return;
            DoctorPollView doctorPollView = new DoctorPollView();
            doctorPollView.DataContext = new DoctorPollViewModel(_patientMedicalRecordViewModel.SelectedAppointment!, _appointmentService, _pollService, _doctorService, _patient);
            doctorPollView.ShowDialog();
            _patientMedicalRecordViewModel.SelectedAppointment = null;
        }
    }
}