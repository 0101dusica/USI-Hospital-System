using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.FacilitiesManagement.Equipments.Model;
using ZdravoCorp.FacilitiesManagement.Equipments.Repository;
using ZdravoCorp.FacilitiesManagement.Equipments.Service;
using ZdravoCorp.FacilitiesManagement.Equipments.ViewModel;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Model;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Model;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Repository;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.FacilitiesManagement.Equipments.Command
{
    public class ChangeEquipmentTableCommand : BaseCommand
    {
        private EquipmentDisplayViewModel _equipmentDisplayViewModel;
        private EquipmentService _equipmentService;
        public ChangeEquipmentTableCommand(EquipmentDisplayViewModel equipmentDisplayViewModel)
        {
            _equipmentDisplayViewModel = equipmentDisplayViewModel;
            _equipmentService = new EquipmentService(new EquipmentRepository(new Serializer<Equipment>()), new RoomRepository(new Serializer<Room>()), new WarehouseRepository(new Serializer<Warehouse>()));
        }

        public override void Execute(object? parameter)
        {
            (List<RoomType> typeOfRooms, List<EquipmentType> typeOfEquipments, List<int> quantities, List<RoomType> warehouses) = _equipmentService.GetSelectedFilters(_equipmentDisplayViewModel);

            bool IsEmptySearchText = string.IsNullOrWhiteSpace(_equipmentDisplayViewModel.SearchText);
            bool IsEmptyFilters = typeOfRooms.Count + typeOfEquipments.Count + quantities.Count + warehouses.Count == 0;

            List<Tuple<Equipment, RoomType, int>> newTable = _equipmentService.GetEquipmentTable()
                .Where(row =>
                    (IsEmptySearchText || _equipmentService.ContainsSearchText(row, _equipmentDisplayViewModel.SearchText.ToLower())) &&
                    (IsEmptyFilters || _equipmentService.MatchesFilters(row, _equipmentDisplayViewModel))
                )
                .ToList();

            _equipmentDisplayViewModel.Equipments = newTable;
        }
    }
}
