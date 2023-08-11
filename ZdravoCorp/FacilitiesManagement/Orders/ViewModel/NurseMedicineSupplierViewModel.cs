using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Model;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Service;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Service;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Model;
using ZdravoCorp.FacilitiesManagement.Orders.Command;

using ZdravoCorp.Utils.ViewModel;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Repository;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Repository;

namespace ZdravoCorp.FacilitiesManagement.Orders.ViewModel
{
    public class NurseMedicineSupplierViewModel : BaseViewModel
    {
        //Bindings
        public List<MedicineItem> MedicineTable { get; set; }
        public WarehouseService WarehouseService;
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

        public ObservableCollection<MedicineItem> MedicineOrderTable { get; set; }
        public MedicineItem SelectedMedicineOrder { get; set; }

        public ICommand AddMedicineOrderCommand { get; }
        public ICommand OrderMedicinesCommand { get; }

        private string _quantity;
        public string Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        //Constructor

        public NurseMedicineSupplierViewModel()
        {
            WarehouseService = new WarehouseService(new WarehouseRepository(new Serializer<Warehouse>()));
            _medicineService = new MedicineService(new MedicineRepository(new Serializer<Medicine>()));
            MedicineTable = WarehouseService.GetMedicinesInDeficit();
            MedicineOrderTable = new ObservableCollection<MedicineItem>();

            AddMedicineOrderCommand = new AddMedicineOrderCommand(this);
            OrderMedicinesCommand = new OrderMedicinesCommand(this);
        }


        //Validation

        public bool isSelectedMedicineValid()
        {
            if (SelectedMedicine == null)
            {
                MessageBox.Show($"Cannot add without selected medicine!");
                return false;
            }
            return true;
        }

        public bool isMedicineOrderTableEmpty()
        {
            if (MedicineOrderTable.Count == 0)
            {
                MessageBox.Show($"Unable to create an empty order!");
                return false;
            }
            return true;
        }

        public bool isQuantityEmpty()
        {
            if (string.IsNullOrEmpty(Quantity))
            {
                MessageBox.Show($"Quantity must be entered!");
                return false;
            }
            return true;
        }

        public bool isMedicineAlreadyOrdered(MedicineItem medicineItem)
        {
            if (MedicineOrderTable.Any(item => item.MedicineId == medicineItem.MedicineId || item.Quantity == medicineItem.Quantity))
            {
                MessageBox.Show($"Already created order!");
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
