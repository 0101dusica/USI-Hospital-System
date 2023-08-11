using System;
using System.Collections.Generic;
using ZdravoCorp.Utils.Command;

using ZdravoCorp.FacilitiesManagement.Orders.ViewModel;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Service;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Model;
using static ZdravoCorp.FacilitiesManagement.Units.Warehouses.Model.MedicineItem;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Repository;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.FacilitiesManagement.Orders.Command
{
    public class AddMedicineOrderCommand : BaseCommand
    {
        private NurseMedicineSupplierViewModel _viewModel;
        private WarehouseService _warehouseService;

        public AddMedicineOrderCommand(NurseMedicineSupplierViewModel viewModel)
        {
            _viewModel = viewModel;
            _warehouseService = new WarehouseService(new WarehouseRepository(new Serializer<Warehouse>()));
        }

        private bool CanExecute(object parameter)
        {

            if (_viewModel.isSelectedMedicineValid() && _viewModel.isMedicineAlreadyOrdered(_viewModel.SelectedMedicine) && _viewModel.isQuantityEmpty())
            {
                return true;
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
                string medicineId = _viewModel.SelectedMedicine.MedicineId;
                int quantity = int.Parse(_viewModel.Quantity);
                ConditionStatus conditionStatus = ConditionStatus.InProcess;
                MedicineItem medicineItem = new MedicineItem(medicineId, quantity, conditionStatus);
                List<MedicineItem> medicineOrders = new List<MedicineItem>();
                medicineOrders = _warehouseService.AddInMedicineOrder(medicineItem);
                foreach (MedicineItem item in medicineOrders)
                {
                    _viewModel.MedicineOrderTable.Add(item);
                }
                _viewModel.Quantity = string.Empty;
            }

        }
    }
}
