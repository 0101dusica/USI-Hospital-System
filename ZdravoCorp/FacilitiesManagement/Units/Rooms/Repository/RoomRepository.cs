using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Model;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.FacilitiesManagement.Equipments.Model;

namespace ZdravoCorp.FacilitiesManagement.Units.Rooms.Repository
{
    public class RoomRepository : IRoomRepository
    {
        private static List<Room> _rooms = new List<Room>();
        private const string _storagePath = "../../../../Data/Rooms.json";

        private ISerializer<Room> _serializer;


        public RoomRepository(ISerializer<Room> serializer)
        {
            _serializer = serializer;
            _rooms = Load();
        }

        public List<Room> Load()
        {
            return _serializer.FromJson(_storagePath);
        }

        public void Save()
        {
            _serializer.ToJson(_storagePath, _rooms);
        }

        public List<Room> GetAll()
        {
            return _rooms;
        }

        public Room? GetById(string id)
        {
            return _rooms.FirstOrDefault(r => r.Id == id);
        }

        public void Add(Room room)
        {
            _rooms.Add(room);
            Save();
        }

        public void Delete(Room room)
        {
            _rooms.Remove(room);
            Save();
        }

        public void Update(Room updatedRoom)
        {
            var existingRoom = GetById(updatedRoom.Id);
            if (existingRoom != null)
            {
                existingRoom.Capacity = updatedRoom.Capacity;
                existingRoom.FreeBeds = updatedRoom.FreeBeds;
                existingRoom.RoomType = updatedRoom.RoomType;
                existingRoom.RoomStatus = updatedRoom.RoomStatus;
                existingRoom.InventoryItems = updatedRoom.InventoryItems;

                Save();
            }
        }

        public string NextId()
        {
            string? lastId = _rooms.LastOrDefault()?.Id;
            if (lastId == null)
            {
                return "room1";
            }
            else
            {
                int lastIdNumber = int.Parse(lastId.Replace("room", ""));
                return $"room{lastIdNumber + 1}";
            }
        }

        public List<string> GetAllIds()
        {
            return _rooms.Select(room => room.Id).ToList();
        }

        public List<InventoryItem> GetInventoryItems(string roomId)
        {
            var room = GetById(roomId);
            if (room != null)
            {
                return room.InventoryItems;
            }
            return new List<InventoryItem>();
        }

        public void UpdateInventoryItem(string roomId, string equipmentId, int newQuantity)
        {
            var room = GetById(roomId);
            var inventoryItem = room?.InventoryItems.FirstOrDefault(item => item.EquipmentId == equipmentId);

            if (inventoryItem != null)
            {
                inventoryItem.Quantity = newQuantity;
                Save();
            }
        }

        public void UpdateStatus(string roomId)
        {
            var room = GetById(roomId);
            if (room != null)
            {
                if (room.RoomStatus == RoomStatus.Avaliable)
                {
                    room.RoomStatus = RoomStatus.InProgress;
                }
                else
                {
                    room.RoomStatus = RoomStatus.Avaliable;
                }

                Save();
            }
        }

    }
}
