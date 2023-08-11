using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Model;
using ZdravoCorp.FacilitiesManagement.Equipments.Repository;
using ZdravoCorp.FacilitiesManagement.Equipments.Model;
using ZdravoCorp.FacilitiesManagement.Renovations.Repository;
using ZdravoCorp.FacilitiesManagement.Renovations.Model;

namespace ZdravoCorp.FacilitiesManagement.Units.Rooms.Service
{
    public class RoomService
    {
        private IRoomRepository _roomRepository;
        private IRenovationRepository _renovationRepository;
        private IEquipmentRepository _equipmentRepository;
        public RoomService(IRoomRepository roomRepository, IRenovationRepository renovationRepository, IEquipmentRepository equipmentRepository)
        {
            _roomRepository = roomRepository;
            _renovationRepository = renovationRepository;
            _equipmentRepository = equipmentRepository;
        }

        public List<Room> GetAll()
        {
            return _roomRepository.GetAll();
        }

        public Room? GetById(string id)
        {
            return _roomRepository.GetById(id);
        }

        public void Add(Room room)
        {
            _roomRepository.Add(room);
        }

        public void Delete(Room room)
        {
            _roomRepository.Delete(room);
        }

        public void Update(Room updatedRoom)
        {
            _roomRepository.Update(updatedRoom);
        }

        public void NextId()
        {
            _roomRepository.NextId();
        }

        public List<string> GetAllIds()
        {
            return _roomRepository.GetAllIds();
        }

        public List<InventoryItem> GetInventoryItems(string roomId)
        {
            return _roomRepository.GetInventoryItems(roomId);
        }

        public void UpdateInventoryItem(string roomId, string equipmentId, int newQuantity)
        {
            _roomRepository.UpdateInventoryItem(roomId, equipmentId, newQuantity);
        }

        public bool IsRoomAvaliable(string roomId)
        {
            return _roomRepository.GetById(roomId).RoomStatus == RoomStatus.Avaliable;
        }

        public List<string> GetExistingRooms()
        {
            List<string> rooms = new List<string>();

            foreach (Room room in _roomRepository.GetAll())
            {
                if (room.RoomStatus == RoomStatus.Avaliable)
                {
                    rooms.Add(room.Id);
                }
            }

            return rooms;
        }

        //need to check with reservations
        public bool IsRoomAvaliableForAppointment(string roomId, DateTime appointmentDate)
        {
            foreach (ComplexRenovation renovation in _renovationRepository.GetAll())
            {
                if (renovation.RoomIds.Contains(roomId) && renovation.TimeSlot.StartTime <= appointmentDate && renovation.TimeSlot.EndTime >= appointmentDate && _roomRepository.GetById(roomId).RoomStatus != RoomStatus.Avaliable)
                {
                    return false;
                }
            }

            return true;
        }

        public List<Tuple<Equipment, int>> GetDynamicEquipments(string roomId)
        {
            return GetById(roomId).InventoryItems
                .SelectMany(item => _equipmentRepository.GetAll()
                    .Where(equipment => equipment.Id == item.EquipmentId && (equipment.EquipmentType == EquipmentType.Appointments || equipment.EquipmentType == EquipmentType.Surgeries))
                    .Select(equipment => Tuple.Create(equipment, item.Quantity)))
                .ToList();
        }
    }
}