using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Model;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Service;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.ViewModel;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Command
{
    public class MarkMedicineOutOfStockCommand : BaseCommand
    {
        private NurseMedicinesReportViewModel _viewModel;
        private WarehouseService _warehouseService;


        public MarkMedicineOutOfStockCommand(NurseMedicinesReportViewModel nurseMedicinesReportViewModel)
        {
            _viewModel = nurseMedicinesReportViewModel;
            _warehouseService = new WarehouseService(new WarehouseRepository(new Serializer<Warehouse>()));
        }

        private bool CanExecute(object parameter)
        {

            return _viewModel.isSelectedMedicine();
        }

        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                _warehouseService.UpdateMedicineItemConditionStatus(_viewModel.SelectedMedicine.MedicineId);
                _viewModel.MedicineTable.Clear();
                var newItems = _warehouseService.GetAllMedicineItems();
                foreach (var item in newItems)
                {
                    _viewModel.MedicineTable.Add(item);
                }
            }
        }
    }
}
