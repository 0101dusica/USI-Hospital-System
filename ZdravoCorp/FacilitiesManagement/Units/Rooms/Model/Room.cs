using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.FacilitiesManagement.Equipments.Model;

namespace ZdravoCorp.FacilitiesManagement.Units.Rooms.Model
{
    public enum RoomType
    {
        OperatingRoom,
        ExaminationRoom,
        PatientRoom,
        WaitingRoom,
        WareHouse
    }

    public enum RoomStatus
    {
        Avaliable,
        InProgress
    }

    public class Room
    {
        public string Id { get; set; }
        public int Capacity { get; set; }
        public int FreeBeds { get; set; }
        public RoomType RoomType { get; set; }
        public RoomStatus RoomStatus { get; set; }
        public List<InventoryItem> InventoryItems { get; set; }

        public Room() { }

        public Room(string id, int capacity, int freeBeds, RoomType roomType, RoomStatus roomStatus, List<InventoryItem> inventoryItems)
        {
            Id = id;
            Capacity = capacity;
            FreeBeds = freeBeds;
            RoomType = roomType;
            RoomStatus = roomStatus;
            InventoryItems = inventoryItems;
        }


    }
}