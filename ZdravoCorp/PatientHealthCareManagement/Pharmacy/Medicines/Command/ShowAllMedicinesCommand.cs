﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Model;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Service;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.View;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.ViewModel;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Command
{
    public class ShowAllMedicinesCommand : BaseCommand
    {
        private NurseMedicationDispenserViewModel _viewModel;
        private WarehouseService _warehouseService;

        public ShowAllMedicinesCommand(NurseMedicationDispenserViewModel viewModel)
        {
            _viewModel = viewModel;
            _warehouseService = new WarehouseService(new WarehouseRepository(new Serializer<Warehouse>()));
        }

        public override void Execute(object? parameter)
        {
            NurseMedicinesReportView nurseMedicinesReportView = new NurseMedicinesReportView();
            nurseMedicinesReportView.DataContext = new NurseMedicinesReportViewModel();
            nurseMedicinesReportView.ShowDialog();
        }
    }
}
