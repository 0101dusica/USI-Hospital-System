using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.SchedulingManagement.Appointments.Repository;
using ZdravoCorp.SchedulingManagement.Appointments.Service;
using ZdravoCorp.UserManagement.Authentication.Menus.MenuPatient.Commands;
using ZdravoCorp.UserManagement.Doctors.Command;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.UserManagement.Doctors.ViewModel
{
    public class PatientSearchDoctorsViewModel : BaseViewModel
    {
        public Patient LoggedPatient { get; set; }
        public DoctorService DoctorService { get; set; }
        public PatientService PatientService { get; set; }
        public AppointmentService AppointmentService { get; set; }
        public bool IsSelected { get; set; }

        public ICommand OrderByNameCommand { get; set; }
        public ICommand OrderByLastNameCommand { get; set; }
        public ICommand OrderByAverageGradeCommand { get; set; }
        public ICommand OrderDoctorBySpecializationCommand { get; set; }
        public ICommand ShowPatientAppointmentsCommand { get; set; }

        private ObservableCollection<Doctor> _doctors = new ObservableCollection<Doctor>();
        public ObservableCollection<Doctor> Doctors
        {
            get => _doctors;
            set
            {
                _doctors = value;
                OnPropertyChanged(nameof(Doctors));
            }
        }

        private string? _keywordName;
        public string? KeywordName
        {
            get => _keywordName;
            set
            {
                _keywordName = value;
                OnPropertyChanged(nameof(KeywordName));
                Doctors = new ObservableCollection<Doctor>(DoctorService.GetSearchName(KeywordName!));
            }
        }

        private string? _keywordLastName;
        public string? KeywordLastName
        {
            get => _keywordLastName;
            set
            {
                _keywordLastName = value;
                OnPropertyChanged(nameof(KeywordLastName));
                Doctors = new ObservableCollection<Doctor>(DoctorService.GetSearchLastName(KeywordLastName!));
            }
        }

        private string? _keywordSpecialization;
        public string? KeywordSpecialization
        {
            get => _keywordSpecialization;
            set
            {
                _keywordSpecialization = value;
                OnPropertyChanged(nameof(KeywordSpecialization));
                Doctors = new ObservableCollection<Doctor>(DoctorService.GetSearchSpecialization(KeywordSpecialization!));
            }
        }
        private Doctor? _selectedDoctor;
        public Doctor? SelectedDoctor
        {
            get => _selectedDoctor;
            set
            {
                _selectedDoctor = value;
                OnPropertyChanged(nameof(SelectedDoctor));
            }
        }

        public PatientSearchDoctorsViewModel(Patient loggedPatient, AppointmentService appointmentService, DoctorService doctorService, PatientService patientService)
        {
            IsSelected = false;

            LoggedPatient = loggedPatient;
            DoctorService = doctorService;
            PatientService = patientService;
            AppointmentService = appointmentService;
            Doctors = new ObservableCollection<Doctor>(doctorService.GetAll());
            OrderByNameCommand = new OrderByNameCommand(this, DoctorService);
            OrderByLastNameCommand = new OrderByLastNameCommand(this, DoctorService);
            OrderByAverageGradeCommand = new OrderByAverageGradeCommand(this, DoctorService);
            OrderDoctorBySpecializationCommand = new OrderDoctorBySpecializationCommand(this, DoctorService);
            ShowPatientAppointmentsCommand = new ShowPatientAppointmentsCommand(this, loggedPatient, doctorService, appointmentService, patientService, new PatientAppointmentActionsService(LoggedPatient, new PatientAppointmentActionsRepository(new Serializer<PatientAppointmentActions>())));
        }

    }
}
