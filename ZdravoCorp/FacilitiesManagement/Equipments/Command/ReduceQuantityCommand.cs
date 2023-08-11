using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.FacilitiesManagement.Equipments.Model;
using ZdravoCorp.FacilitiesManagement.Equipments.Repository;
using ZdravoCorp.FacilitiesManagement.Equipments.ViewModel;
using ZdravoCorp.FacilitiesManagement.Renovations.Model;
using ZdravoCorp.FacilitiesManagement.Renovations.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Model;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Service;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.FacilitiesManagement.Equipments.Command
{
    public class ReduceQuantityCommand : BaseCommand
    {
        private DoctorEquipmentUsageViewModel _viewModel;


        private RoomService _roomService;
        public event EventHandler<bool> QuantityReduced;

        public ReduceQuantityCommand(DoctorEquipmentUsageViewModel viewModel)
        {
            _viewModel = viewModel;
            _roomService = new RoomService(new RoomRepository(new Serializer<Room>()), new RenovationRepository(new Serializer<ComplexRenovation>()), new EquipmentRepository(new Serializer<Equipment>())) ;
        }

        private bool CanExecute(object parameter)
        {
            return _viewModel.SelectedEquipmentWithQuantity != null && _viewModel.SelectedEquipmentWithQuantity.Item2 >= _viewModel.TakenQuantity
;
        }
        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                int newQuantity = _viewModel.SelectedEquipmentWithQuantity.Item2 - _viewModel.TakenQuantity;
                //_roomService.UpdateInventoryItem(_viewModel.Appointment.RoomId, _viewModel.SelectedEquipmentWithQuantity.Item1.Id,newQuantity);
                _viewModel.EquipmentsWithQuantityTable = _viewModel.LoadEquipments(_viewModel.RoomId);

                QuantityReduced?.Invoke(this, true);
            }
        }
    }
}
