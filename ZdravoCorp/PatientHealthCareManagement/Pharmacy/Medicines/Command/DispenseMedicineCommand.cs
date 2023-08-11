using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Model;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Service;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Model;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Repository;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Service;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.ViewModel;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Model;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Repository;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Service;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Command
{
    public class DispenseMedicineCommand : BaseCommand
    {
        private WarehouseService _warehouseService;
        private PrescriptionService _prescriptionService;
        private MedicineService _medicineService;
        private NurseMedicationDispenserViewModel _viewModel;

        public DispenseMedicineCommand(NurseMedicationDispenserViewModel viewModel)
        {
            _warehouseService = new WarehouseService(new WarehouseRepository(new Serializer<Warehouse>()));
            _medicineService = new MedicineService(new MedicineRepository(new Serializer<Medicine>()));
            _prescriptionService = new PrescriptionService(new PrescriptionRepository(new Serializer<Prescription>()));
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            if (_viewModel.isPatientSelected() && _viewModel.isPrescriptionSelected())
            {
                string medicineId = _viewModel.SelectedPrescription.MedicineId;
                if (_warehouseService.isMedicineOnCondition(medicineId))
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("The medicine is currently out of stock!", "Out of Stock");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                DateTime StartTime = _viewModel.SelectedPrescription.TimeSlot.StartTime;
                DateTime EndTime = _viewModel.SelectedPrescription.TimeSlot.EndTime;
                TimeSpan difference = EndTime.Subtract(StartTime);

                DateTime today = DateTime.Now.Date;
                today = today.AddSeconds(-today.Second);

                DateTime newStartTime = today;
                DateTime newEndTime = today.AddDays(difference.Days);

                _prescriptionService.UpdatePrescriptionTimeSlot(_viewModel.SelectedPrescription.Id, newStartTime, newEndTime);

                string medicineId = _viewModel.SelectedPrescription.MedicineId;
                _warehouseService.ReduceMedicineQuantity(medicineId);
                Medicine medicine = _medicineService.GetById(medicineId);
                MedicineItem? medicineItem = _warehouseService.GetMedicineItemById(medicineId);
                MessageBox.Show("Medicine successfully dispensed!\nInformations:\n" + medicineId + ": " + medicine.Name + "\nCurrently in stock: " + medicineItem.Quantity + "\n" + "Updated period of use of the medicine!\n" + "StartTime: " + newStartTime.ToString("dd/MM/yyy") + "\nEndTime: " + newEndTime.ToString("dd/MM/yyyy"));

                _viewModel.PrescriptionTable.Clear();
                var prescriptions = _prescriptionService.GetPrescriptionsEndingByTomorrow(_viewModel.SelectedPatient.Username);
                foreach (var prescription in prescriptions)
                {
                    _viewModel.PrescriptionTable.Add(prescription);
                }
            }
        }
    }
}
