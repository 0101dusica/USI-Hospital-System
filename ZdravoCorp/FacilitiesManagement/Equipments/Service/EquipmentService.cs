using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.FacilitiesManagement.Equipments.Repository;
using ZdravoCorp.FacilitiesManagement.Equipments.Model;
using ZdravoCorp.FacilitiesManagement.Equipments.ViewModel;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Model;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Model;

namespace ZdravoCorp.FacilitiesManagement.Equipments.Service
{
    public class EquipmentService
    {
        private IEquipmentRepository _equipmentRepository;
        private IRoomRepository _roomRepository;
        private IWarehouseRepository _warehouseRepository;
        public EquipmentService(IEquipmentRepository equipmentRepository, IRoomRepository roomRepository, IWarehouseRepository warehouseRepository)
        {
            _equipmentRepository = equipmentRepository;
            _roomRepository = roomRepository;
            _warehouseRepository = warehouseRepository;
        }

        public List<Equipment> GetAll()
        {
            return _equipmentRepository.GetAll();
        }

        public Equipment? GetById(string id)
        {
            return _equipmentRepository.GetById(id);
        }

        public void Add(Equipment equipment)
        {
            _equipmentRepository.Add(equipment);
        }

        public void Delete(Equipment equipment)
        {
            _equipmentRepository.Delete(equipment);
        }

        public void Update(Equipment updatedEquipment)
        {
            _equipmentRepository.Update(updatedEquipment);
        }

        public void NextId()
        {
            _equipmentRepository.NextId();
        }


        public List<Room> GetRooms(string equipmentId)
        {
            return _roomRepository.GetAll().Where(room => room.InventoryItems.Any(item => item.EquipmentId == equipmentId)).ToList();
        }

        public List<Warehouse> GetWarehouses(string equipmentId)
        {
            return _warehouseRepository.GetAll().Where(warehouse => warehouse.InventoryItems.Any(item => item.EquipmentId == equipmentId)).ToList();
        }
        public int GetQuantity(string equipmentId, string roomId)
        {
            return _roomRepository.GetAll().FirstOrDefault(room => room.Id == roomId)?.InventoryItems.FirstOrDefault(item => equipmentId.Equals(item.EquipmentId))?.Quantity ?? 0;
        }

        public List<Tuple<Equipment, RoomType, int>> GetEquipmentTable()
        {
            return _equipmentRepository.GetAll()
                        .SelectMany(equipment => GetRooms(equipment.Id)
                        .Select(room => new Tuple<Equipment, RoomType, int>(equipment, room.RoomType, GetQuantity(equipment.Id, room.Id))))
                        .ToList()
                    .Concat(
                    _equipmentRepository.GetAll()
                        .SelectMany(equipment => GetWarehouses(equipment.Id)
                            .Select(warehouse => new Tuple<Equipment, RoomType, int>(equipment, RoomType.WareHouse, GetQuantity(equipment.Id, warehouse.Id))))
                        .ToList()
                    ).ToList();
        }

        public List<Tuple<Equipment, int>> GetCountedEquipment()
        {
            var firstList = _equipmentRepository.GetAll()
                .Select(equipment => new Tuple<Equipment, int>(
                    equipment,
                    _roomRepository.GetAll()
                        .SelectMany(room => room.InventoryItems)
                        .Where(item => item.EquipmentId == equipment.Id)
                        .Sum(item => item.Quantity)))
                .ToList();

            var secondList = _equipmentRepository.GetAll()
                .Select(equipment => new Tuple<Equipment, int>(
                    equipment,
                    _warehouseRepository.GetAll()
                        .SelectMany(warehouse => warehouse.InventoryItems)
                        .Where(item => item.EquipmentId == equipment.Id)
                        .Sum(item => item.Quantity)))
                .ToList();

            var resultList = firstList.Concat(secondList.Where(tuple => !firstList.Any(t => t.Item1.Id == tuple.Item1.Id)))
                .GroupBy(tuple => tuple.Item1.Id)
                .Select(group =>
                    new Tuple<Equipment, int>(
                        group.First().Item1,
                        group.Sum(tuple => tuple.Item2)))
                .ToList();

            return resultList;

        }

        public List<Tuple<Equipment, int>> GetDeficitDynamicEquipment()
        {
            return GetCountedEquipment()
                .Where(equipment => (equipment.Item1.EquipmentType == EquipmentType.Appointments || equipment.Item1.EquipmentType == EquipmentType.Surgeries) && equipment.Item2 <= 5)
                .ToList();
        }

        public List<Tuple<Equipment, int>> GetEquipmentByRoom(bool isDynamic, string roomId)
        {
            List<Tuple<Equipment, int>> result = new List<Tuple<Equipment, int>>();

            Room room = _roomRepository.GetById(roomId);
            Warehouse warehouse = _warehouseRepository.GetById(roomId);

            List<InventoryItem> items = room != null ? room.InventoryItems : warehouse?.InventoryItems;

            if (items != null)
            {
                foreach (InventoryItem item in items)
                {
                    Equipment equipment = _equipmentRepository.GetById(item.EquipmentId);
                    if (equipment != null)
                    {
                        bool isAppointmentsOrSurgeries = equipment.EquipmentType == EquipmentType.Appointments || equipment.EquipmentType == EquipmentType.Surgeries;
                        bool isRoomFurnitureOrHallwayEquipments = equipment.EquipmentType == EquipmentType.RoomFurniture || equipment.EquipmentType == EquipmentType.HallwayEquipments;

                        if (isAppointmentsOrSurgeries && isDynamic || isRoomFurnitureOrHallwayEquipments && !isDynamic)
                        {
                            result.Add(new Tuple<Equipment, int>(equipment, item.Quantity));
                        }
                    }
                }
            }

            return result;
        }

        public bool ContainsSearchText(Tuple<Equipment, RoomType, int> row, string searchText)
        {
            return row.Item1.Name.ToLower().Contains(searchText) ||
                row.Item1.Id.ToLower().Contains(searchText) ||
                row.Item1.EquipmentType.ToString().ToLower().Contains(searchText) ||
                row.Item2.ToString().ToLower().Contains(searchText) ||
                row.Item3.ToString().Contains(searchText);
        }

        public bool MatchesFilters(Tuple<Equipment, RoomType, int> row, EquipmentDisplayViewModel _equipmentDisplayViewModel)
        {
            (List<RoomType> typeOfRooms, List<EquipmentType> typeOfEquipments, List<int> quantities, List<RoomType> warehouses) = GetSelectedFilters(_equipmentDisplayViewModel);

            return (typeOfRooms.Count == 0 || typeOfRooms.Contains(row.Item2)) &&
                (typeOfEquipments.Count == 0 || typeOfEquipments.Contains(row.Item1.EquipmentType)) &&
                (warehouses.Count == 0 || warehouses.Contains(row.Item2)) &&
                (quantities.Count == 0 ||
                    row.Item3 == 0 && quantities.Contains(0) ||
                    row.Item3 > 0 && row.Item3 <= 10 && quantities.Contains(1) ||
                    row.Item3 > 10 && quantities.Contains(2)
                );
        }

        public (List<RoomType>, List<EquipmentType>, List<int>, List<RoomType>) GetSelectedFilters(EquipmentDisplayViewModel _equipmentDisplayViewModel)
        {
            List<RoomType> typeOfRooms = new List<RoomType>();
            List<EquipmentType> typeOfEqipments = new List<EquipmentType>();
            List<int> quantities = new List<int>();
            List<RoomType> warehouses = new List<RoomType>();

            if (_equipmentDisplayViewModel.IsOperatingRoomChecked == true)
            {
                typeOfRooms.Add(RoomType.OperatingRoom);
            }
            if (_equipmentDisplayViewModel.IsExaminationRoomChecked == true)
            {
                typeOfRooms.Add(RoomType.ExaminationRoom);
            }
            if (_equipmentDisplayViewModel.IsPatientRoomChecked == true)
            {
                typeOfRooms.Add(RoomType.PatientRoom);
            }
            if (_equipmentDisplayViewModel.IsWaitingRoomChecked == true)
            {
                typeOfRooms.Add(RoomType.WaitingRoom);
            }

            if (_equipmentDisplayViewModel.IsAppointmentsChecked == true)
            {
                typeOfEqipments.Add(EquipmentType.Appointments);
            }
            if (_equipmentDisplayViewModel.IsSurgeriesChecked == true)
            {
                typeOfEqipments.Add(EquipmentType.Surgeries);
            }
            if (_equipmentDisplayViewModel.IsRoomFurnitureChecked == true)
            {
                typeOfEqipments.Add(EquipmentType.RoomFurniture);
            }
            if (_equipmentDisplayViewModel.IsHallwayEquipmentsChecked == true)
            {
                typeOfEqipments.Add(EquipmentType.HallwayEquipments);
            }
            if (_equipmentDisplayViewModel.IsZeroChecked == true)
            {
                quantities.Add(0);
            }
            if (_equipmentDisplayViewModel.IsLessThan10Checked == true)
            {
                quantities.Add(1);
            }
            if (_equipmentDisplayViewModel.IsMoreThan10Checked == true)
            {
                quantities.Add(2);
            }

            if (_equipmentDisplayViewModel.IsOutsideChecked == true)
            {
                warehouses.Add(RoomType.OperatingRoom);
                warehouses.Add(RoomType.ExaminationRoom);
                warehouses.Add(RoomType.PatientRoom);
                warehouses.Add(RoomType.WaitingRoom);
            }
            if (_equipmentDisplayViewModel.IsInsideChecked == true)
            {
                warehouses.Add(RoomType.WareHouse);
            }

            return (typeOfRooms, typeOfEqipments, quantities, warehouses);
        }

    }
}
