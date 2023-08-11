using System.Collections.Generic;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Model;
using ZdravoCorp.FacilitiesManagement.Equipments.Model;

namespace ZdravoCorp.FacilitiesManagement.Units.Rooms.Repository
{
    public interface IRoomRepository
    {
        List<Room> GetAll();
        Room? GetById(string id);
        void Add(Room room);
        void Delete(Room room);
        void Update(Room updatedRoom);
        string NextId();
        List<string> GetAllIds();
        List<InventoryItem> GetInventoryItems(string roomId);
        void UpdateInventoryItem(string roomId, string equipmentId, int newQuantity);
        void UpdateStatus(string roomId);
    }
}
