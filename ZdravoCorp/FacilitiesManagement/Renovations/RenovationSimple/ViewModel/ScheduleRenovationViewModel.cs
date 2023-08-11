using System;
using System.Collections.Generic;
using System.Windows.Input;

using ZdravoCorp.Utils.ViewModel;
using ZdravoCorp.FacilitiesManagement.Renovations.RenovationSimple.Command;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Service;
using ZdravoCorp.FacilitiesManagement.Renovations.RenovationSimple.View;
using ZdravoCorp.FacilitiesManagement.Equipments.Model;
using ZdravoCorp.FacilitiesManagement.Equipments.Repository;
using ZdravoCorp.FacilitiesManagement.Renovations.Model;
using ZdravoCorp.FacilitiesManagement.Renovations.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Model;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Repository;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.FacilitiesManagement.Renovations.RenovationSimple.ViewModel
{
    public class ScheduleRenovationViewModel : BaseViewModel
    {
        public ScheduleRenovationView ScheduleRenovationView { get; set; }

        public List<string> RenovationRoom { get; set; }

        private string _selectedRenovationRoom;
        public string SelectedRenovationRoom
        {
            get { return _selectedRenovationRoom; }
            set { _selectedRenovationRoom = value; }
        }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        private bool _isOneRoomChecked;
        public bool IsOneRoomChecked
        {
            get { return _isOneRoomChecked; }
            set
            {
                _isOneRoomChecked = value;
                OnPropertyChanged();
                if (value)
                {
                    IsMergeRoomsChecked = false;
                    IsSeparateRoomsChecked = false;
                }
            }
        }

        private bool _isMergeRoomsChecked;
        public bool IsMergeRoomsChecked
        {
            get { return _isMergeRoomsChecked; }
            set
            {
                _isMergeRoomsChecked = value;
                OnPropertyChanged();
                if (value)
                {
                    IsOneRoomChecked = false;
                    IsSeparateRoomsChecked = false;
                }
            }
        }

        private bool _isSeparateRoomsChecked;
        public bool IsSeparateRoomsChecked
        {
            get { return _isSeparateRoomsChecked; }
            set
            {
                _isSeparateRoomsChecked = value;
                OnPropertyChanged();
                if (value)
                {
                    IsOneRoomChecked = false;
                    IsMergeRoomsChecked = false;
                }
            }
        }
        public ICommand SubmitButton { get; }

        public ScheduleRenovationViewModel() { }


        public ScheduleRenovationViewModel(ScheduleRenovationView scheduleRenovationView)
        {
            ScheduleRenovationView = scheduleRenovationView;
            RenovationRoom = new RoomService(new RoomRepository(new Serializer<Room>()), new RenovationRepository(new Serializer<ComplexRenovation>()), new EquipmentRepository(new Serializer<Equipment>())).GetExistingRooms();

            SubmitButton = new AddSimpleRenovationCommand(this);
        }
    }
}
