using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.CommunicatonManagement.Chats.Command;
using ZdravoCorp.UserManagement;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Doctors.Repository;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.UserManagement.Nurses.Model;
using ZdravoCorp.UserManagement.Nurses.Repository;
using ZdravoCorp.UserManagement.Nurses.Service;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.CommunicatonManagement.Chats.ViewModel
{
    public class ChooseRecipientViewModel : BaseViewModel
    {

        public User LoggedUser { get; set; }

        public ICommand ShowChatCommand { get; }

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

        private ObservableCollection<Nurse> _nurses = new ObservableCollection<Nurse>();
        public ObservableCollection<Nurse> Nurses
        {
            get => _nurses;
            set
            {
                _nurses = value;
                OnPropertyChanged(nameof(Nurses));
            }
        }

        private Nurse? _selectedNurse;
        public Nurse? SelectedNurse
        {
            get => _selectedNurse;
            set
            {
                _selectedNurse = value;
                OnPropertyChanged(nameof(SelectedNurse));

            }
        }

        private DoctorService _doctorService;
        private NurseService _nurseService;

        public ChooseRecipientViewModel(User loggedUser)
        {
            LoggedUser = loggedUser;
            _doctorService = new DoctorService(new DoctorRepository(new Serializer<Doctor>()));
            _nurseService = new NurseService(new NurseRepository(new Serializer<Nurse>()));
            FillDoctorComboBox();
            FillNurseDoctorComboBox();
            ShowChatCommand = new ShowChatCommand(this);


        }

        public void FillNurseDoctorComboBox()
        {
            if (LoggedUser is Nurse)
            {
                Nurse nurse = (Nurse)LoggedUser;
                Nurses = new ObservableCollection<Nurse>(_nurseService.GetAllNursesExceptLoggedIn(nurse));
            }
            else
            {
                Nurses = new ObservableCollection<Nurse>(_nurseService.GetAll());
            }
        }

        public void FillDoctorComboBox()
        {
            if (LoggedUser is Doctor)
            {
                Doctor doctor = (Doctor)LoggedUser;
                Doctors = new ObservableCollection<Doctor>(_doctorService.GetAllNursesExceptLoggedIn(doctor));
            }
            else
            {
                Doctors = new ObservableCollection<Doctor>(_doctorService.GetAll());
            }
            
        }

        public bool CheckIfBothSelected()
        {
            if (SelectedDoctor != null && SelectedNurse != null)
            {
                MessageBox.Show("You can not choose 2 users for conversation!");
                SelectedDoctor = null;
                SelectedNurse = null;
                return true;

            }
            return false;
        }

        public bool CheckIfBothNotSelected()
        {

            if (SelectedDoctor == null && SelectedNurse == null)
            {
                MessageBox.Show("You must chose one user from one of those combo boxes!");
                return true;

            }
            return false;
        }

        public User CheckWhoIsSelected()
        {
            if (SelectedDoctor != null) return SelectedDoctor;
            return SelectedNurse!;

        }
    }
}
