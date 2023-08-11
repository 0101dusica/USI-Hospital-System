using System;
using System.Collections.Generic;
using System.Windows.Input;
using ZdravoCorp.FacilitiesManagement.Equipments.Service;
using ZdravoCorp.FacilitiesManagement.Equipments.Model;
using ZdravoCorp.Utils.ViewModel;
using ZdravoCorp.FacilitiesManagement.Equipments.Command;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Model;
using ZdravoCorp.FacilitiesManagement.Equipments.View;
using ZdravoCorp.FacilitiesManagement.Equipments.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Model;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Repository;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.FacilitiesManagement.Equipments.ViewModel
{
    public class EquipmentDisplayViewModel : BaseViewModel
    {
        private EquipmentService _equipmentService;
        private List<Tuple<Equipment, RoomType, int>> _equipments;

        public List<Tuple<Equipment, RoomType, int>> Equipments
        {
            get { return _equipments; }
            set
            {
                _equipments = value;
                OnPropertyChanged(nameof(Equipments));
            }
        }

        public List<RoomType> RoomTypes { get; set; }

        public List<int> Quantities { get; set; }

        private bool _isOperatingRoomChecked;
        public bool IsOperatingRoomChecked
        {
            get { return _isOperatingRoomChecked; }
            set { _isOperatingRoomChecked = value; OnPropertyChanged(); ApplyChanegs(); }
        }

        private bool _isExaminationRoomChecked;
        public bool IsExaminationRoomChecked
        {
            get { return _isExaminationRoomChecked; }
            set { _isExaminationRoomChecked = value; OnPropertyChanged(); ApplyChanegs(); }
        }

        private bool _isPatientRoomChecked;
        public bool IsPatientRoomChecked
        {
            get { return _isPatientRoomChecked; }
            set { _isPatientRoomChecked = value; OnPropertyChanged(); ApplyChanegs(); }
        }

        private bool _isWaitingRoomChecked;
        public bool IsWaitingRoomChecked
        {
            get { return _isWaitingRoomChecked; }
            set { _isWaitingRoomChecked = value; OnPropertyChanged(); ApplyChanegs(); }
        }

        private bool _isAppointmentsChecked;
        public bool IsAppointmentsChecked
        {
            get { return _isAppointmentsChecked; }
            set { _isAppointmentsChecked = value; OnPropertyChanged(); ApplyChanegs(); }
        }

        private bool _isSurgeriesChecked;
        public bool IsSurgeriesChecked
        {
            get { return _isSurgeriesChecked; }
            set { _isSurgeriesChecked = value; OnPropertyChanged(); ApplyChanegs(); }
        }

        private bool _isRoomFurnitureChecked;
        public bool IsRoomFurnitureChecked
        {
            get { return _isRoomFurnitureChecked; }
            set { _isRoomFurnitureChecked = value; OnPropertyChanged(); ApplyChanegs(); }
        }

        private bool _isHallwayEquipmentsChecked;
        public bool IsHallwayEquipmentsChecked
        {
            get { return _isHallwayEquipmentsChecked; }
            set { _isHallwayEquipmentsChecked = value; OnPropertyChanged(); ApplyChanegs(); }
        }

        private bool _isZeroChecked;
        public bool IsZeroChecked
        {
            get { return _isZeroChecked; }
            set { _isZeroChecked = value; OnPropertyChanged(); ApplyChanegs(); }
        }

        private bool _isLessThan10Checked;
        public bool IsLessThan10Checked
        {
            get { return _isLessThan10Checked; }
            set { _isLessThan10Checked = value; OnPropertyChanged(); ApplyChanegs(); }
        }

        private bool _isMoreThan10Checked;
        public bool IsMoreThan10Checked
        {
            get { return _isMoreThan10Checked; }
            set
            {
                _isMoreThan10Checked = value; OnPropertyChanged(); ApplyChanegs();
            }
        }

        private bool _isInsideChecked;
        public bool IsInsideChecked
        {
            get { return _isInsideChecked; }
            set
            {
                _isInsideChecked = value; OnPropertyChanged(); ApplyChanegs();
            }
        }

        private bool _isOutsideChecked;
        public bool IsOutsideChecked
        {
            get { return _isOutsideChecked; }
            set
            {
                _isOutsideChecked = value;
                OnPropertyChanged();
                ApplyChanegs(); // Corrected placement
            }
        }
        public ICommand ResetFilters { get; }

        public EquipmentDisplayView EquipmentDisplayView { get; set; }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                ApplyChanegs();
            }
        }

        public EquipmentDisplayViewModel(EquipmentDisplayView equipmentDisplayView)
        {
            _equipmentService = new EquipmentService(new EquipmentRepository(new Serializer<Equipment>()), new RoomRepository(new Serializer<Room>()), new WarehouseRepository(new Serializer<Warehouse>()));
            EquipmentDisplayView = equipmentDisplayView;

            Equipments = _equipmentService.GetEquipmentTable();
            ResetFilters = new ResetCheckBoxCommand(this);
        }


        public void ApplyChanegs()
        {
            var command = new ChangeEquipmentTableCommand(this);
            command.Execute(null);
        }
    }
}
