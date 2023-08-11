using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Command;
using ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.Command;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.ViewModel
{
    public class AddHospitalReferralViewModel : BaseViewModel
    {
        public string PatientUsername { get; set; }
        public string DoctorUsername { get; set; }


        private ObservableCollection<string> _additionalTests = new ObservableCollection<string>();
        public ObservableCollection<string> AdditionalTests
        {
            get { return _additionalTests; }
            set { _additionalTests = value; OnPropertyChanged(nameof(AdditionalTests)); }
        }

        private string _newAdditionalTest;
        public string NewAdditionalTest
        {
            get { return _newAdditionalTest; }
            set { _newAdditionalTest = value; OnPropertyChanged(nameof(NewAdditionalTest)); }
        }

        private string _selectedAdditionalTest;
        public string SelectedAdditionalTest
        {
            get { return _selectedAdditionalTest; }
            set
            {
                _selectedAdditionalTest = value;
                OnPropertyChanged(nameof(SelectedAdditionalTest));
            }
        }

        private int _days;
        public int Days
        {
            get { return _days; }
            set
            {
                _days = value;
                OnPropertyChanged(nameof(Days));
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        private Appointment _appointment;

        public Appointment Appointment
        {
            get { return _appointment; }
            set { _appointment = value; }
        }


        public ICommand ShowPrescriptionViewCommand { get; }
        public ICommand AddPrescriptionCommand { get; }
        public ICommand AddAdditionalTestCommand { get; }
        public ICommand DeleteAdditionalTestCommand { get; }
        public ICommand AddHospitalReferralCommand { get; }


        public AddHospitalReferralViewModel(Appointment appointment)
        {
            PatientUsername = appointment.PatientUsername;
            DoctorUsername = appointment.DoctorUsername;

            SelectedDate = DateTime.Now;

            AddHospitalReferralCommand = new AddHospitalReferralCommand(this);
            ((AddHospitalReferralCommand)AddHospitalReferralCommand).HospitalReferralAdded += OnHospitalReferralAdded;

            ShowPrescriptionViewCommand = new ShowPrescriptionViewCommand<AddHospitalReferralViewModel>(this);
            AddAdditionalTestCommand = new AddAdditionalTestCommand(this);
            DeleteAdditionalTestCommand = new DeleteAdditionalTestCommand(this);
        }

        private void OnHospitalReferralAdded(object? sender, bool success)
        {
            if (success)
            {
                MessageBox.Show($"Hospital referral added successfully for {PatientUsername}.");

            }
            else { MessageBox.Show($"Please input valid data."); }
        }
    }
}
