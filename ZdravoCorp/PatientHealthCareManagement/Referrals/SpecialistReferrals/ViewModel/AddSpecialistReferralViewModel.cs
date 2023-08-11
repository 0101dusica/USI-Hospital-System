using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows;
using ZdravoCorp.Utils.ViewModel;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.PatientHealthCareManagement.Referrals.SpecialistReferrals.Command;
using ZdravoCorp.UserManagement.Doctors.Repository;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PatientHealthCareManagement.Referrals.SpecialistReferrals.ViewModel
{
    public class AddSpecialistReferralViewModel : BaseViewModel
    {
        private Doctor _loggedDoctor;
        public Doctor LoggedDoctor
        {
            get { return _loggedDoctor; }
        }
        private string _patientUsername;
        public string PatientUsername
        {
            get { return _patientUsername; }
        }

        private ObservableCollection<Doctor> _doctorsTable;
        public ObservableCollection<Doctor> DoctorsTable
        {
            get { return _doctorsTable; }
            set
            {
                _doctorsTable = value;
                OnPropertyChanged(nameof(DoctorsTable));
            }
        }
        private Doctor _selectedDoctor;
        public Doctor SelectedDoctor
        {
            get { return _selectedDoctor; }
            set
            {
                _selectedDoctor = value;
                OnPropertyChanged(nameof(SelectedDoctor));
            }
        }
        public IEnumerable<Specialization> Specializations => Enum.GetValues(typeof(Specialization)).Cast<Specialization>();

        private Specialization _selectedSpecialization;
        public Specialization SelectedSpecialization
        {
            get { return _selectedSpecialization; }
            set
            {
                _selectedSpecialization = value;
                OnPropertyChanged(nameof(SelectedSpecialization));

                GetBySpecialization(_selectedSpecialization);
            }
        }

        public ICommand AddSpecialistReferralCommand { get; }
        public AddSpecialistReferralViewModel(Doctor loggedDoctor, string patientUsername)
        {
            DoctorService ds = new DoctorService(new DoctorRepository(new Serializer<Doctor>()));

            _loggedDoctor = loggedDoctor;
            _patientUsername = patientUsername;


            AddSpecialistReferralCommand = new AddSpecialistReferralCommand(this);
            ((AddSpecialistReferralCommand)AddSpecialistReferralCommand).SpecialistReferralAdded += OnSpecialistReferralAdded;
            DoctorsTable = new ObservableCollection<Doctor>(ds.GetWithoutLoggedDoctor(_loggedDoctor));
        }
        private void OnSpecialistReferralAdded(object sender, bool success)
        {
            if (success)
            {
                if (SelectedDoctor != null)
                {
                    MessageBox.Show($"Specialist referral added successfully for {_patientUsername}, referred to {SelectedDoctor.Username}.");
                }
                else
                {
                    MessageBox.Show($"Specialist referral added successfully for {SelectedSpecialization}.");
                }
            }
            else
            {
                MessageBox.Show($"Specialist does not exist.");
            }
        }
        public void GetBySpecialization(Specialization selectedSpecialization)
        {
            DoctorService doctorService = new DoctorService(new DoctorRepository(new Serializer<Doctor>()));
            DoctorsTable = new ObservableCollection<Doctor>(doctorService.GetWithoutLoggedDoctor(_loggedDoctor)
                .Where(d => d.Specialization == selectedSpecialization));
        }

    }
}