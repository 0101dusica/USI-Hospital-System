using System;
using System.Collections.Generic;
using System.Windows.Input;

using ZdravoCorp.Utils.ViewModel;
using ZdravoCorp.FacilitiesManagement.Equipments.Service;
using ZdravoCorp.FacilitiesManagement.Equipments.Model;
using ZdravoCorp.FacilitiesManagement.Transfers.Model;
using ZdravoCorp.FacilitiesManagement.Transfers.View;
using ZdravoCorp.FacilitiesManagement.Transfers.Command;
using ZdravoCorp.FacilitiesManagement.Equipments.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Model;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Model;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Repository;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.FacilitiesManagement.Transfers.ViewModel
{
    public class DistributionOrderViewModel : BaseViewModel
    {
        private EquipmentService _equipmentService { get; set; }
        public Transfer Transfer { get; set; }
        public List<Tuple<Equipment, int>> EquipmentsFromRoom { get; set; }
        public List<Tuple<Equipment, int>> EquipmentsToRoom { get; set; }
        public int? QuantityInput { get; set; }
        public DateTime DateInput { get; set; }
        public bool IsDatePickerEnabled => Transfer.TransferType != TransferType.Instant;
        public ICommand SaveOrder { get; }
        public ICommand SubmitOrder { get; }

        private Tuple<Equipment, int> _selectedRow;
        public Tuple<Equipment, int> SelectedRow
        {
            get { return _selectedRow; }
            set
            {
                _selectedRow = value;
                OnPropertyChanged(nameof(SelectedRow));
            }
        }

        public DistributionOrderView DistributionOrderView { get; set; }
        public DistributionOrderViewModel() { }
        public DistributionOrderViewModel(DistributionOrderView distributionOrderView, Transfer transfer)
        {
            _equipmentService = new EquipmentService(new EquipmentRepository(new Serializer<Equipment>()), new RoomRepository(new Serializer<Room>()), new WarehouseRepository(new Serializer<Warehouse>()));

            Transfer = transfer;

            DistributionOrderView = distributionOrderView;

            if (transfer.TransferType == TransferType.Instant)
            {
                EquipmentsFromRoom = _equipmentService.GetEquipmentByRoom(true, transfer.FromRoomId);
                EquipmentsToRoom = _equipmentService.GetEquipmentByRoom(true, transfer.ToRoomId);
            }
            else
            {
                EquipmentsFromRoom = _equipmentService.GetEquipmentByRoom(false, transfer.FromRoomId);
                EquipmentsToRoom = _equipmentService.GetEquipmentByRoom(false, transfer.ToRoomId);
            }

            SaveOrder = new SaveTransferCommand(this);
            //this.SubmitOrder = new RelayCommand(SubmitOrderButton);
        }

        //private void SubmitOrderButton(object obj)
        //{
        //    if (Transfer.InventoryItems.Count == 0)
        //    {
        //        MessageBox.Show("You didn't input equipments that you want to transfer!");
        //    }
        //    else
        //    {
        //        if (this.Transfer.TransferType == TransferType.Deferred)
        //        {
        //            this.Transfer.TimeSlot.EndTime = this.DateInput;
        //            DelayChangeQuantityEquipment();
        //        }
        //        else
        //        {
        //            DelayChangeQuantityEquipment();
        //        }

        //        this.MainStorage.Transfers.Add(this.Transfer);
        //        this.MainStorage.transferStorage.Save(this.MainStorage.Transfers);

        //        if (this.Transfer.TransferType == TransferType.Instant)
        //        {
        //            this.Transfer.UpdateInventoryItems(this.MainStorage, this.Transfer);
        //        }

        //        MessageBox.Show("You just successfully made a transfer!");

        //        new EquipmentDistributionView(this.MainStorage).Show();
        //        this.DistributionOrderView.Close();
        //    }
        //}

        //public void DelayChangeQuantityEquipment()
        //{
        //    foreach (InventoryItem transferItem in this.Transfer.InventoryItems)
        //    {
        //        foreach (Hospital hospital in this.MainStorage.Hospitals)
        //        {
        //            foreach (Warehouse warehouse in hospital.Warehouses)
        //            {
        //                if (warehouse.Id == this.Transfer.FromRoomId)
        //                {
        //                    foreach (InventoryItem inventoryItem in warehouse.InventoryItems)
        //                    {
        //                        if (inventoryItem.EquipmentId == transferItem.EquipmentId)
        //                        {
        //                            inventoryItem.Quantity = inventoryItem.Quantity - transferItem.Reserved;
        //                            inventoryItem.Reserved = transferItem.Reserved;
        //                        }
        //                    }
        //                }
        //            }
        //            foreach (Room room in hospital.Rooms)
        //            {
        //                if (room.Id == this.Transfer.FromRoomId)
        //                {
        //                    foreach (InventoryItem inventoryItem in room.InventoryItems)
        //                    {
        //                        if (inventoryItem.EquipmentId == transferItem.EquipmentId)
        //                        {
        //                            inventoryItem.Quantity = inventoryItem.Quantity - transferItem.Reserved;
        //                            inventoryItem.Reserved = transferItem.Reserved;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    this.MainStorage.hospitalStorage.Save(this.MainStorage.Hospitals);
        //}

    }
}
