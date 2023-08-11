using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

using ZdravoCorp.Utils.ViewModel;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Service;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Service;
using ZdravoCorp.FacilitiesManagement.Transfers.View;
using ZdravoCorp.FacilitiesManagement.Transfers.Command;
using ZdravoCorp.FacilitiesManagement.Transfers.Service;
using ZdravoCorp.FacilitiesManagement.Transfers.Model;
using ZdravoCorp.FacilitiesManagement.Transfers.Repository;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.FacilitiesManagement.Equipments.Model;
using ZdravoCorp.FacilitiesManagement.Equipments.Repository;
using ZdravoCorp.FacilitiesManagement.Renovations.Model;
using ZdravoCorp.FacilitiesManagement.Renovations.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Model;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Model;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Repository;

namespace ZdravoCorp.FacilitiesManagement.Transfers.ViewModel
{
    public class EquipmentDistributionViewModel : BaseViewModel
    {
        private RoomService _roomService { get; set; }
        private WarehouseService _warehouseService { get; set; }
        private TransferService _transferService { get; set; }
        public EquipmentDistributionView EquipmentDistributionView { get; set; }

        private List<string> _fromRoom;
        public List<string> FromRoom
        {
            get { return _fromRoom; }
            set
            {
                _fromRoom = value;
            }
        }

        private string _selectedFromRoom;
        public string SelectedFromRoom
        {
            get { return _selectedFromRoom; }
            set { _selectedFromRoom = value; }
        }

        private List<string> _toRoom;
        public List<string> ToRoom
        {
            get { return _toRoom; }
            set
            {
                _toRoom = value;
            }
        }

        private string _selectedToRoom;
        public string SelectedToRoom
        {
            get { return _selectedToRoom; }
            set { _selectedToRoom = value; }
        }

        private bool _isDyinamicChecked;
        public bool IsDyinamicChecked
        {
            get { return _isDyinamicChecked; }
            set { _isDyinamicChecked = value; OnPropertyChanged(); }
        }

        public Transfer Transfer { get; set; }
        public string UrgentRooms { get; set; } //display urgernt rooms, rooms that has 0 equipments 
        public ICommand SubmitButton { get; }

        public EquipmentDistributionViewModel(EquipmentDistributionView equipmentDistributionView)
        {
            EquipmentDistributionView = equipmentDistributionView;
            _roomService = new RoomService(new RoomRepository(new Serializer<Room>()), new RenovationRepository(new Serializer<ComplexRenovation>()), new EquipmentRepository(new Serializer<Equipment>()));
            _warehouseService = new WarehouseService(new WarehouseRepository(new Serializer<Warehouse>()));
            _transferService = new TransferService(new TransferRepository(new Serializer<Transfer>()));

            FromRoom = _roomService.GetAllIds().Concat(_warehouseService.GetAllIds()).ToList();
            ToRoom = _roomService.GetAllIds().Concat(_warehouseService.GetAllIds()).ToList();

            SubmitButton = new ShowDistributionViewCommand(this);
        }
    }
}
