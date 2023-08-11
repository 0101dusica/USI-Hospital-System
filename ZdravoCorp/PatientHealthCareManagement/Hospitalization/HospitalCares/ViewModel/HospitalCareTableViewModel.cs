using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.PatientHealthCareManagement.HospitalCares.Repository;
using ZdravoCorp.PatientHealthCareManagement.HospitalCares.Service;
using ZdravoCorp.PatientHealthCareManagement.Hospitalization.HospitalCares.Command;
using ZdravoCorp.PatientHealthCareManagement.Hospitalization.HospitalCares.Model;
using ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.Model;
using ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.Repository;
using ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.Service;
using ZdravoCorp.PatientHealthCareManagement.Visits.Model;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Repository;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.PatientHealthCareManagement.Hospitalization.HospitalCares.ViewModel
{
    public class HospitalCareTableViewModel : BaseViewModel
    {

        private ObservableCollection<HospitalCare> hospitalCares;
        public ObservableCollection<HospitalCare> HospitalCares
        {
            get { return hospitalCares; }
            set
            {
                hospitalCares = value;
                OnPropertyChanged(nameof(HospitalCares));
            }
        }

        private ObservableCollection<Visit> visits;
        public ObservableCollection<Visit> Visits
        {
            get { return visits; }
            set
            {
                visits = value;
                OnPropertyChanged(nameof(Visits));
            }
        }

        private HospitalCare selectedHospitalCare;
        public HospitalCare SelectedHospitalCare
        {
            get { return selectedHospitalCare; }
            set
            {
                selectedHospitalCare = value;
                OnPropertyChanged(nameof(SelectedHospitalCare));
                LoadVisits(selectedHospitalCare);

            }
        }

        
        private Doctor _loggedDoctor;
        public Doctor LoggedDoctor
        {
            get { return _loggedDoctor; }
        }

        public ICommand VisitPatientCommand { get; }

        public HospitalCareTableViewModel(Doctor loggedDoctor)
        {
            HospitalCares = LoadHospitalCares(); ;
            _loggedDoctor = loggedDoctor;

            VisitPatientCommand = new VisitPatientCommand(this);
            ((VisitPatientCommand)VisitPatientCommand).VisitStarted += OnVisitStarted;



        }

        private void OnVisitStarted(object? sender, bool success)
        {
            if (!success)
            {
                MessageBox.Show("Please select a hospital care in progress option for the patient and make sure that hospital care is available for today.");
            }
        }

        public ObservableCollection<HospitalCare> LoadHospitalCares()
        {
            HospitalCareService service = new HospitalCareService(new HospitalCareRepository(new Serializer<HospitalCare>()));
            return new ObservableCollection<HospitalCare>(service.GetAll());
        }

        private void LoadVisits(HospitalCare selectedHospitalCare)
        {
            HospitalCareService service = new HospitalCareService(new HospitalCareRepository(new Serializer<HospitalCare>()));
            Visits = new ObservableCollection<Visit>(service.GetVisitsForHospitalCare(SelectedHospitalCare));
        }

    }
}
