using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.FacilitiesManagement.Equipments.Service;
using ZdravoCorp.FacilitiesManagement.Equipments.Model;
using ZdravoCorp.Utils.ViewModel;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Service;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.FacilitiesManagement.Equipments.Command;
using ZdravoCorp.FacilitiesManagement.Equipments.Repository;
using ZdravoCorp.FacilitiesManagement.Renovations.Model;
using ZdravoCorp.FacilitiesManagement.Renovations.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Model;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Repository;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.FacilitiesManagement.Equipments.ViewModel
{
    public class DoctorEquipmentUsageViewModel : BaseViewModel
    {
        private ObservableCollection<Tuple<Equipment, int>> _equipmentsWithQuantityTable;
        public ObservableCollection<Tuple<Equipment, int>> EquipmentsWithQuantityTable
        {
            get { return _equipmentsWithQuantityTable; }
            set
            {
                _equipmentsWithQuantityTable = value;
                OnPropertyChanged(nameof(EquipmentsWithQuantityTable));
            }
        }

        private Tuple<Equipment, int> _selectedEquipmentWithQuantity;
        public Tuple<Equipment, int> SelectedEquipmentWithQuantity
        {
            get { return _selectedEquipmentWithQuantity; }
            set
            {
                _selectedEquipmentWithQuantity = value;
                OnPropertyChanged(nameof(SelectedEquipmentWithQuantity));

                TakenQuantity = SelectedEquipmentWithQuantity == null ? 0 : SelectedEquipmentWithQuantity.Item2;
            }
        }

        private int _takenQuantity;
        public int TakenQuantity
        {
            get { return _takenQuantity; }
            set
            {
                _takenQuantity = value;
                OnPropertyChanged(nameof(TakenQuantity));
            }
        }
        public Appointment Appointment { get; }

        public string RoomId { get; set; }
        public ICommand ReduceQuantityCommand { get; }

        public DoctorEquipmentUsageViewModel(string roomId)
        {

            RoomId = roomId;
            _takenQuantity = 0;

            EquipmentsWithQuantityTable = LoadEquipments(roomId);


            ReduceQuantityCommand = new ReduceQuantityCommand(this);
            ((ReduceQuantityCommand)ReduceQuantityCommand).QuantityReduced += OnQuantityReduced;
        }

        private void OnQuantityReduced(object sender, bool success)
        {
            if (success)
            {
                EquipmentsWithQuantityTable = LoadEquipments(RoomId);
            }
            else
            {
                MessageBox.Show("Not enough quantity available to reduce..");
            }
        }
        public ObservableCollection<Tuple<Equipment, int>> LoadEquipments(string roomId)
        {
            RoomService roomService = new RoomService(new RoomRepository(new Serializer<Room>()), new RenovationRepository(new Serializer<ComplexRenovation>()), new EquipmentRepository(new Serializer<Equipment>()));
            return new ObservableCollection<Tuple<Equipment, int>>(roomService.GetDynamicEquipments(roomId));
        }



    }
}
