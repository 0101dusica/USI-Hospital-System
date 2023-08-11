using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using ZdravoCorp.Utils.Command;
using ZdravoCorp.FacilitiesManagement.Transfers.ViewModel;
using ZdravoCorp.FacilitiesManagement.Transfers.Service;
using ZdravoCorp.FacilitiesManagement.Equipments.Model;
using ZdravoCorp.SchedulingManagement;
using ZdravoCorp.FacilitiesManagement.Transfers.Model;
using ZdravoCorp.FacilitiesManagement.Transfers.View;
using ZdravoCorp.FacilitiesManagement.Transfers.Repository;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.FacilitiesManagement.Transfers.Command
{
    public class ShowDistributionViewCommand : BaseCommand
    {
        private EquipmentDistributionViewModel _equipmentDistributionViewModel;
        private TransferService _transferService;
        public ShowDistributionViewCommand(EquipmentDistributionViewModel equipmentDisplayViewModel)
        {
            _equipmentDistributionViewModel = equipmentDisplayViewModel;
            _transferService = new TransferService(new TransferRepository(new Serializer<Transfer>()));
        }

        public bool CanExecute(object parameter)
        {
            string fromRoom = _equipmentDistributionViewModel.SelectedFromRoom;
            string toRoom = _equipmentDistributionViewModel.SelectedToRoom;
            if (fromRoom == toRoom)
            {
                MessageBox.Show("You can't distribute eqipments into the same room!");
                return false;
            }
            else
            {
                return true;
            }

        }
        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                List<InventoryItem> transferItems = new List<InventoryItem>();
                TimeSlot timeSlot = new TimeSlot();
                timeSlot.StartTime = DateTime.Now;
                timeSlot.EndTime = DateTime.Now;

                _equipmentDistributionViewModel.Transfer = new Transfer(_transferService.NextId(), _equipmentDistributionViewModel.SelectedFromRoom, _equipmentDistributionViewModel.SelectedToRoom, transferItems, TransferType.Instant, TransferStatus.InProcess, timeSlot);

                if (_equipmentDistributionViewModel.IsDyinamicChecked == false)
                {
                    _equipmentDistributionViewModel.Transfer.TransferType = TransferType.Deferred;
                }

                _transferService.Add(_equipmentDistributionViewModel.Transfer);

                DistributionOrderView distributionOrderView = new DistributionOrderView();
                distributionOrderView.DataContext = new DistributionOrderViewModel(distributionOrderView, _equipmentDistributionViewModel.Transfer);
                distributionOrderView.ShowDialog();
            }
        }
    }
}
