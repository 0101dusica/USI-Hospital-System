using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ZdravoCorp.FacilitiesManagement.Equipments.Model;
using ZdravoCorp.SchedulingManagement;

namespace ZdravoCorp.FacilitiesManagement.Transfers.Model
{
    public enum TransferType
    {
        Deferred,
        Instant
    }

    public enum TransferStatus
    {
        InProcess,
        Done
    }
    public class Transfer
    {
        public string Id { get; set; }
        public string FromRoomId { get; set; }
        public string ToRoomId { get; set; }
        public List<InventoryItem> InventoryItems { get; set; }
        public TransferType TransferType { get; set; }
        public TransferStatus TransferStatus { get; set; }
        public TimeSlot TimeSlot { get; set; }

        public Transfer() { }

        public Transfer(string id, string fromRoomId, string toRoomId, List<InventoryItem> inventoryItems, TransferType type, TransferStatus transferStatus, TimeSlot timeSlot)
        {
            Id = id;
            FromRoomId = fromRoomId;
            ToRoomId = toRoomId;
            InventoryItems = inventoryItems;
            TransferType = type;
            TransferStatus = transferStatus;
            TimeSlot = timeSlot;

        }
    }

}
