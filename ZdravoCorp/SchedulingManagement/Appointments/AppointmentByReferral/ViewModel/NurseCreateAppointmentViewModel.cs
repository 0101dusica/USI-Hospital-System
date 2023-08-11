using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.PatientHealthCareManagement.Referrals.SpecialistReferrals.Model;
using ZdravoCorp.SchedulingManagement.Appointments.AppointmentByReferral.Command;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Repository;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.SchedulingManagement.Appointments.AppointmentByReferral.ViewModel
{
    public class NurseCreateAppointmentViewModel : BaseViewModel
    {
        //Bindings

        public List<Patient> PatientsTable { get; set; }

        public IEnumerable<AppointmentType> AppointmentTypes => Enum.GetValues(typeof(AppointmentType))
            .Cast<AppointmentType>()
            .Where(type => type == AppointmentType.Appointment || type == AppointmentType.Surgery);

        public Patient SelectedPatient { get; set; }

        public DateTime SelectedDateTime { get; set; }

        public ObservableCollection<SpecialistReferral> ReferralTable { get; set; }
        public SpecialistReferral SelectedReferral { get; set; }

        public ICommand CreateNurseAppointmentCommand { get; }
        public ICommand GetPatientReferralCommand { get; }

        private string _surgeryDuration;
        public string SurgeryDuration
        {
            get { return _surgeryDuration; }
            set
            {
                _surgeryDuration = value;
                OnPropertyChanged(nameof(SurgeryDuration));
            }
        }

        private Visibility _surgeryDurationVisibility;
        public Visibility SurgeryDurationVisibility
        {
            get { return _surgeryDurationVisibility; }
            set
            {
                _surgeryDurationVisibility = value;
                OnPropertyChanged(nameof(SurgeryDurationVisibility));
            }
        }
        private AppointmentType _selectedAppointmentType;
        public AppointmentType SelectedAppointmentType
        {
            get { return _selectedAppointmentType; }
            set
            {
                _selectedAppointmentType = value;
                OnPropertyChanged(nameof(SelectedAppointmentType));
                SurgeryDurationVisibility = _selectedAppointmentType == AppointmentType.Surgery ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        //Constructor

        public NurseCreateAppointmentViewModel()
        {
            PatientService patientService = new PatientService(new PatientRepository(new Serializer<Patient>()));
            PatientsTable = patientService.GetAll();
            ReferralTable = new ObservableCollection<SpecialistReferral>();
            SurgeryDurationVisibility = Visibility.Collapsed;
            DateTime currentTime = DateTime.Now;
            currentTime = currentTime.AddSeconds(-currentTime.Second);
            SelectedDateTime = currentTime;

            GetPatientReferralCommand = new GetPatientReferralCommand(this);
            CreateNurseAppointmentCommand = new CreateNurseAppointmentCommand(this);

        }

        //Validations

        public bool isPatientSelected()
        {
            if (SelectedPatient != null)
            {
                return true;
            }
            MessageBox.Show($"Cannot submit without selected patient!");
            return false;

        }

        public bool isReferralSelected()
        {
            if (SelectedReferral != null)
            {
                return true;
            }
            MessageBox.Show($"Cannot submit without selected referral!");
            return false;
        }
        public bool isDatePassed()
        {
            DateTime currentDateTime = DateTime.Now;
            currentDateTime = currentDateTime.AddSeconds(-currentDateTime.Second);
            if (SelectedDateTime < currentDateTime)
            {
                MessageBox.Show($"The selected date has passed!");
                return false;
            }
            return true;
        }

        public bool isSugeryDurationIsValid()
        {
            if (SelectedAppointmentType.Equals(AppointmentType.Surgery))
            {
                bool valid = int.TryParse(SurgeryDuration, out _);
                if (!valid)
                {
                    MessageBox.Show($"Surgery duration time is empty!");
                }
                return valid;
            }
            else
            {
                return true;
            }
        }




    }
}
