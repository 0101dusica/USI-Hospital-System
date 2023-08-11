using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Command;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Model;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Repository;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Service;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Command;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Model;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Repository;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.ViewModel
{
    public class NurseMedicationDispenserViewModel : BaseViewModel
    {
        //Bindings

        private MedicineService _medicineService;
        private PatientService _patientService;
        public List<Patient> PatientsTable { get; set; }

        public Patient SelectedPatient { get; set; }

        public ObservableCollection<Prescription> PrescriptionTable { get; set; }
        private Prescription _selectedPrescription;
        public Prescription SelectedPrescription
        {
            get { return _selectedPrescription; }
            set
            {
                if (SetProperty(ref _selectedPrescription, value))
                {
                    UpdateMedicineName();
                    UpdatePrescriptionEndTime();
                }
            }
        }

        private string _medicineName;
        public string MedicineName
        {
            get { return _medicineName; }
            set
            {
                _medicineName = value;
                OnPropertyChanged(nameof(MedicineName));
            }
        }

        private string _prescriptionEndTime;
        public string PrescriptionEndTime
        {
            get { return _prescriptionEndTime; }
            set
            {
                _prescriptionEndTime = value;
                OnPropertyChanged(nameof(PrescriptionEndTime));
            }
        }

        public DateTime SelectedDateTime { get; set; }

        public ICommand GetPatientsPrescriptionsCommand { get; }
        public ICommand DispenseMedicineCommand { get; }
        public ICommand ShowMedicinesInDeficitCommand { get; }
        public ICommand ShowAllMedicinesCommand { get; }
        public ICommand CreatePrescriptionAppointmentCommand { get; }

        //Constructor

        public NurseMedicationDispenserViewModel()
        {
            _patientService = new PatientService(new PatientRepository(new Serializer<Patient>()));
            _medicineService = new MedicineService(new MedicineRepository(new Serializer<Medicine>()));
            PatientsTable = _patientService.GetAll();
            PrescriptionTable = new ObservableCollection<Prescription>();
            DateTime currentTime = DateTime.Now;
            currentTime = currentTime.AddSeconds(-currentTime.Second);
            SelectedDateTime = currentTime;

            GetPatientsPrescriptionsCommand = new GetPatientsPrescriptionsCommand(this);
            DispenseMedicineCommand = new DispenseMedicineCommand(this);
            ShowMedicinesInDeficitCommand = new ShowMedicinesInDeficitCommand(this);
            ShowAllMedicinesCommand = new ShowAllMedicinesCommand(this);
            //CreatePrescriptionAppointmentCommand = new CreatePrescriptionAppointmentCommand(this); //mozda obrisana comanda
        }

        //Validations

        public bool isPatientSelected()
        {
            if (SelectedPatient == null)
            {
                MessageBox.Show($"Cannot submit without selected patient!");
                return false;
            }
            return true;
        }

        public bool isPrescriptionSelected()
        {
            if (SelectedPrescription == null)
            {
                MessageBox.Show($"Cannot submit without selected prescription!");
                return false;
            }
            return true;
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

        //functions that update txt fields on a selection in a table

        private void UpdateMedicineName()
        {
            if (SelectedPrescription != null)
            {
                Medicine? medicine = _medicineService.GetById(SelectedPrescription.MedicineId);
                MedicineName = medicine?.Name;
            }
            else
            {
                MedicineName = string.Empty;
            }
        }

        private void UpdatePrescriptionEndTime()
        {
            if (SelectedPrescription != null)
            {
                PrescriptionEndTime = SelectedPrescription.TimeSlot.EndTime.ToString("dd/MM/yyyy");
            }
            else
            {
                PrescriptionEndTime = string.Empty;
            }
        }
    }
}
