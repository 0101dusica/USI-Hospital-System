using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Model;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Service;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Command;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.View;
using ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.View;
using ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.ViewModel;
using ZdravoCorp.PatientHealthCareManagement.Referrals.SpecialistReferrals.View;
using ZdravoCorp.PatientHealthCareManagement.Referrals.SpecialistReferrals.ViewModel;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.ViewModel;
using ZdravoCorp.SchedulingManagement.Appointments.Examination.Command;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.ViewModel;
using ZdravoCorp.FacilitiesManagement.Equipments.Model;
using ZdravoCorp.FacilitiesManagement.Equipments.Repository;
using ZdravoCorp.FacilitiesManagement.Renovations.Model;
using ZdravoCorp.FacilitiesManagement.Renovations.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Repository;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.SchedulingManagement.Appointments.Examination.ViewModel
{
    public class DoctorExaminationViewModel : BaseViewModel
    {
        public Appointment Appointment;

        private string _observation;
        public string Observation
        {
            get { return _observation; }
            set
            {
                if (_observation != value)
                {
                    _observation = value;
                    OnPropertyChanged(nameof(Observation));
                }
            }
        }
        private ObservableCollection<Room> _rooms;
        public ObservableCollection<Room> Rooms
        {
            get { return _rooms; }
            set { _rooms = value; OnPropertyChanged(nameof(Rooms)); }
        }
        private Room _selectedRoom;
        public Room SelectedRoom
        {
            get { return _selectedRoom; }
            set
            {
                if (_selectedRoom != value)
                {
                    _selectedRoom = value;
                    OnPropertyChanged(nameof(SelectedRoom));
                }
            }
        }

        private ObservableCollection<string> _symptoms = new ObservableCollection<string>();
        public ObservableCollection<string> Symptoms
        {
            get { return _symptoms; }
            set { _symptoms = value; OnPropertyChanged(nameof(Symptoms)); }
        }

        private string _newSymptom;
        public string NewSymptom
        {
            get { return _newSymptom; }
            set { _newSymptom = value; OnPropertyChanged(nameof(NewSymptom)); }
        }

        private string _selectedSymptom;
        public string SelectedSymptom
        {
            get { return _selectedSymptom; }
            set
            {
                if (_selectedSymptom != value)
                {
                    _selectedSymptom = value;
                    OnPropertyChanged(nameof(SelectedSymptom));
                }
            }
        }

        private Doctor _loggedDoctor;
        public ICommand AddSymptomCommand { get; }
        public ICommand DeleteSymptomCommand { get; }
        public ICommand ShowMedicalRecordCommand { get; }
        public ICommand EndExaminationCommand { get; }
        public ICommand ShowSpecialistRefferalCommand { get; }
        public ICommand ShowUpdateMedicalRecordCommand { get; }
        public ICommand ShowHospitalReferralCommand { get; }
        public ICommand ShowPrescriptionViewCommand { get; }

        private Window _view;
        public Window View
        {
            get { return _view; }
        }
        public DoctorExaminationViewModel(Window view, Appointment appointment, Doctor loggedDoctor)
        {
            RoomService rs = new RoomService(new RoomRepository(new Serializer<Room>()), new RenovationRepository(new Serializer<ComplexRenovation>()), new EquipmentRepository(new Serializer<Equipment>())); //promenices
            Appointment = appointment;
            _loggedDoctor = loggedDoctor;
            _view = view;

            Symptoms = appointment.Anamnesis.Symptoms != null ? new ObservableCollection<string>(appointment.Anamnesis.Symptoms) : new ObservableCollection<string>();

            Rooms = new ObservableCollection<Room>(rs.GetAll());
            AddSymptomCommand = new AddSypmtomCommand(this);
            DeleteSymptomCommand = new DeleteSypmtomCommand(this);

            ShowSpecialistRefferalCommand = new RelayCommand(ShowSpecialistRefferal);
            ShowPrescriptionViewCommand = new RelayCommand(ShowPrescription);
            ShowHospitalReferralCommand = new RelayCommand(ShowHospitalReferral);

            EndExaminationCommand = new EndExaminationCommand(this);
            ((EndExaminationCommand)EndExaminationCommand).ExaminationEnded += OnExaminationEnded;


            ShowMedicalRecordCommand = new ShowMedicalRecordCommand<DoctorExaminationViewModel>(this, OnResult);
            ShowUpdateMedicalRecordCommand = new ShowUpdateMedicalRecordCommand<DoctorExaminationViewModel>(this, OnResult);
        }
        private void ShowHospitalReferral()
        {
            var view = new AddHospitalReferralView();
            view.DataContext = new AddHospitalReferralViewModel(Appointment);
            view.ShowDialog();
        }

        private void OnExaminationEnded(object? sender, bool success)
        {
            if (!success)
            {
                MessageBox.Show("Please input valid data.");
            }
        }

        private void ShowSpecialistRefferal()
        {
            var view = new AddSpecialistReferralView();
            view.DataContext = new AddSpecialistReferralViewModel(_loggedDoctor, Appointment.PatientUsername);
            view.ShowDialog();
        }
        private void ShowPrescription()
        {
            var view = new AddPrescriptionView();
            view.DataContext = new AddPrescriptionViewModel(Appointment);
            view.ShowDialog();
        }
        private void OnResult(bool success)
        {
            if (!success)
            {
                MessageBox.Show("Error.");
            }
        }
    }
}
