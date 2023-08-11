using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Model;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Repository;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Service;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Command;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.ViewModel
{
    public class AddPrescriptionViewModel : BaseViewModel
    {
        private int _dailyUsage;
        public int DailyUsage
        {
            get { return _dailyUsage; }
            set
            {
                _dailyUsage = value;
                OnPropertyChanged(nameof(Duration));
            }
        }
        private ObservableCollection<Medicine> _medicineTable;
        public ObservableCollection<Medicine> MedicineTable
        {
            get { return _medicineTable; }
            set
            {
                _medicineTable = value;
                OnPropertyChanged(nameof(MedicineTable));
            }
        }

        private Medicine _selectedMedicine;
        public Medicine SelectedMedicine
        {
            get { return _selectedMedicine; }
            set
            {
                _selectedMedicine = value;
                OnPropertyChanged(nameof(SelectedMedicine));
            }
        }

        private Instruction _selectedInstruction;
        public Instruction SelectedInstruction
        {
            get { return _selectedInstruction; }
            set
            {
                _selectedInstruction = value;
                OnPropertyChanged(nameof(SelectedInstruction));

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

        private TimeSpan _time;
        public TimeSpan Time
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged(nameof(Time));
            }
        }

        private int _duration;
        public int Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                OnPropertyChanged(nameof(Duration));
            }
        }
        public IEnumerable<Instruction> Instructions => Enum.GetValues(typeof(Instruction)).Cast<Instruction>();

        public Appointment Appointment { get; set; }
        public ICommand AddPrescriptionCommand { get; }
        public ICommand CheckAllergieCommand { get; }
        public AddPrescriptionViewModel(Appointment appointment)
        {
            MedicineService ms = new MedicineService(new MedicineRepository(new Serializer<Medicine>()));
            MedicineTable = new ObservableCollection<Medicine>(ms.GetAll());

            SelectedDate = DateTime.Now;
            Appointment = appointment;

            CheckAllergieCommand = new CheckAllergieCommand(this);
            ((CheckAllergieCommand)CheckAllergieCommand).AllergiesChecked += OnAllergiesChecked;

            AddPrescriptionCommand = new AddPrescriptionCommand(this);
            ((AddPrescriptionCommand)AddPrescriptionCommand).PrescriptionAdded += OnPrescriptionAdded;
        }

        private void OnPrescriptionAdded(object? sender, bool success)
        {
            MessageBox.Show(success ? "Prescription added successfully." : "Failed to add prescription. Please try again.",
                            success ? "Success" : "Error",
                            MessageBoxButton.OK,
                            success ? MessageBoxImage.Information : MessageBoxImage.Error);
        }

        private void OnAllergiesChecked(object? sender, bool success)
        {
            if (success)
            {
                MessageBox.Show($"Patient is allergic to ingredients of {SelectedMedicine.Name}. Please consider an alternative medication.", "Allergy Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show("Patient is not allergic to the selected medicine.", "Allergy Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
