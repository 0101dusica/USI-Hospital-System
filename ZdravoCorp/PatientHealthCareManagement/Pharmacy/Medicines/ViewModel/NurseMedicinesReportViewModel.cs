using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Model;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Service;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Command;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Model;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Repository;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Service;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.ViewModel

{
    public class NurseMedicinesReportViewModel : BaseViewModel
    {
        public ObservableCollection<MedicineItem> MedicineTable { get; set; }
        private MedicineService _medicineService;

        private MedicineItem _selectedMedicine;
        public MedicineItem SelectedMedicine
        {
            get { return _selectedMedicine; }
            set
            {
                if (SetProperty(ref _selectedMedicine, value))
                {
                    UpdateMedicineName();
                }
            }
        }

        public ICommand MarkMedicineOutOfStockCommand { get; }

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

        public NurseMedicinesReportViewModel()
        {
            WarehouseService warehouseService = new WarehouseService(new WarehouseRepository(new Serializer<Warehouse>()));
            _medicineService = new MedicineService(new MedicineRepository(new Serializer<Medicine>()));

            MedicineTable = new ObservableCollection<MedicineItem>(warehouseService.GetAllMedicineItems());
            MarkMedicineOutOfStockCommand = new MarkMedicineOutOfStockCommand(this);
        }

        public bool isSelectedMedicine()
        {
            if (SelectedMedicine == null)
            {
                MessageBox.Show("You have to choose a medicine!");
                return false;
            }

            if (SelectedMedicine.Quantity > 0)
            {
                MessageBox.Show("Selected medicine must have a quantity of 0.");
                SelectedMedicine = null;
                return false;
            }

            return true;
        }
        private void UpdateMedicineName()
        {
            if (SelectedMedicine != null)
            {
                Medicine? medicine = _medicineService.GetById(SelectedMedicine.MedicineId);
                MedicineName = medicine?.Name;
            }
            else
            {
                MedicineName = string.Empty;
            }
        }

    }
}
