using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using ZdravoCorp.Utils.Command;
using ZdravoCorp.FacilitiesManagement.Orders.ViewModel;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Service;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Model;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Repository;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.FacilitiesManagement.Orders.Command
{
    public class OrderMedicinesCommand : BaseCommand
    {
        private NurseMedicineSupplierViewModel _viewModel;
        private WarehouseService _warehouseService;

        public OrderMedicinesCommand(NurseMedicineSupplierViewModel viewModel)
        {
            _viewModel = viewModel;
            _warehouseService = new WarehouseService(new WarehouseRepository(new Serializer<Warehouse>()));
        }

        private bool CanExecute(object parameter)
        {

            return _viewModel.isMedicineOrderTableEmpty();
        }

        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                foreach (MedicineItem medicineItem in _viewModel.MedicineOrderTable)
                {
                    try
                    {
                        _warehouseService.AddInMedicineItems(medicineItem);
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine($"Error accessing JSON file: {ex.Message}");
                        Thread.Sleep(TimeSpan.FromHours(24));
                        _warehouseService.AddInMedicineItems(medicineItem);
                    }
                }

                MessageBox.Show($"Successful order that will be realized in 24 hours!");
            }
        }
    }
}
