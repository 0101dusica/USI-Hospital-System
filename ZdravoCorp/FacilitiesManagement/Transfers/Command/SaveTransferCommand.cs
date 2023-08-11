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
using ZdravoCorp.FacilitiesManagement.Transfers.Repository;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.FacilitiesManagement.Transfers.Model;

namespace ZdravoCorp.FacilitiesManagement.Transfers.Command
{
    public class SaveTransferCommand : BaseCommand
    {
        private DistributionOrderViewModel _distributionOrderViewModel;
        private TransferService _transferService;
        public SaveTransferCommand(DistributionOrderViewModel distributionOrderViewModel)
        {
            _distributionOrderViewModel = distributionOrderViewModel;
            _transferService = new TransferService(new TransferRepository(new Serializer<Transfer>()));
        }

        private bool CanExecute(object parameter)
        {
            if (_distributionOrderViewModel.QuantityInput <= 0)
            {
                MessageBox.Show("You can't transfer less than 1 equipment!");
                return false;
            }
            else if (_distributionOrderViewModel.QuantityInput > _distributionOrderViewModel.SelectedRow.Item2)
            {
                MessageBox.Show("You don't have as many as required eqipment in this room!");
                return false;
            }
            else if (_distributionOrderViewModel.DateInput < DateTime.Now)
            {
                MessageBox.Show("You can't scheadule transfer for past time!");
                return false;
            }
            return true;
        }

        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                InventoryItem transferItem = new InventoryItem(_distributionOrderViewModel.SelectedRow.Item1.Id, _distributionOrderViewModel.SelectedRow.Item2, (int)_distributionOrderViewModel.QuantityInput);

                if (_transferService.AddInventoryItem(_distributionOrderViewModel.Transfer.Id, transferItem) != true)
                {
                    MessageBox.Show("You don't have as many as required eqipment in this room!");
                }
                else
                {
                    MessageBox.Show($"You add {_distributionOrderViewModel.SelectedRow.Item1.Name} to Transfer order!");
                }
            }
        }
    }
}
