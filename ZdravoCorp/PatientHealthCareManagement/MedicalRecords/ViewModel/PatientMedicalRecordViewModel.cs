using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ZdravoCorp.CommunicatonManagement.Polls.Service;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Command;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Service;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.PatientHealthCareManagement.MedicalRecords.ViewModel
{
    public class PatientMedicalRecordViewModel : BaseViewModel
    {
        public Patient LoggedPatient { get; set; }
        public AppointmentService AppointmentService { get; set; }
        public ICommand DateSortCommand { get; set; }
        public ICommand DoctorSortCommand { get; set; }
        public ICommand SpecializationSortCommand { get; set; }
        public ICommand ShowDoctorPollCommand { get; set; }

        private ObservableCollection<Appointment>? _appointments;
        public ObservableCollection<Appointment>? Appointments
        {
            get => _appointments;
            set
            {
                _appointments = value;
                OnPropertyChanged(nameof(Appointments));

            }
        }

        private string? _keyword;
        public string? Keyword
        {
            get => _keyword;
            set
            {
                _keyword = value;
                OnPropertyChanged(nameof(Keyword));
                Appointments = new ObservableCollection<Appointment>(AppointmentService.GetSearchAnamnesis(Keyword!, LoggedPatient));
            }
        }

        private Appointment? _selectedAppointment;
        public Appointment? SelectedAppointment
        {
            get => _selectedAppointment;
            set
            {
                _selectedAppointment = value;
                OnPropertyChanged(nameof(SelectedAppointment));

            }
        }

        public PatientMedicalRecordViewModel(Patient loggedPatient, DoctorService doctorService,
            AppointmentService appointmentService, PatientService patientService, PollService pollService)
        {
            LoggedPatient = loggedPatient;
            AppointmentService = appointmentService;
            Appointments = new ObservableCollection<Appointment>(appointmentService.GetPatientFinishedAppointments(loggedPatient));
            DateSortCommand = new OrderByDateCommand(this, appointmentService);
            DoctorSortCommand = new OrderByDoctorCommand(this, doctorService);
            SpecializationSortCommand = new OrderAppointmentsBySpecializationCommand(this, appointmentService, doctorService);
            ShowDoctorPollCommand = new ShowDoctorPollCommand(this, appointmentService, pollService, doctorService, loggedPatient);
        }
    }
}
