using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using ZdravoCorp.FacilitiesManagement.Transfers.Model;
using ZdravoCorp.Utils.Serializer;
namespace ZdravoCorp.FacilitiesManagement.Transfers.Repository
{
    public class TransferRepository : ITransferRepository
    {
        private static List<Transfer> _transfers = new List<Transfer>();
        private const string _storagePath = "../../../Data/Transfers.json";

        private ISerializer<Transfer> _serializer;


        public TransferRepository(ISerializer<Transfer> serializer)
        {
            _serializer = serializer;
            _transfers = Load();
        }

        public List<Transfer> Load()
        {
            return _serializer.FromJson(_storagePath);
        }

        public void Save()
        {
            _serializer.ToJson(_storagePath, _transfers);
        }

        public List<Transfer> GetAll()
        {
            return _transfers;
        }

        public Transfer? GetById(string id)
        {
            return _transfers.FirstOrDefault(t => t.Id.Equals(id));
        }

        public void Add(Transfer transfer)
        {
            _transfers.Add(transfer);
            Save();
        }

        public void Delete(Transfer transfer)
        {
            _transfers.Remove(transfer);
            Save();
        }

        public void Update(Transfer updatedTransfer)
        {
            var existingTransfer = GetById(updatedTransfer.Id);
            if (existingTransfer != null)
            {
                existingTransfer.FromRoomId = updatedTransfer.FromRoomId;
                existingTransfer.ToRoomId = updatedTransfer.ToRoomId;
                existingTransfer.InventoryItems = updatedTransfer.InventoryItems;
                existingTransfer.TransferType = updatedTransfer.TransferType;
                existingTransfer.TransferStatus = updatedTransfer.TransferStatus;
                existingTransfer.TimeSlot = updatedTransfer.TimeSlot;

                Save();
            }
        }

        public string NextId()
        {
            string? lastId = _transfers.LastOrDefault()?.Id;
            if (lastId == null)
            {
                return "transfer1";
            }
            else
            {
                int lastIdNumber = int.Parse(lastId.Replace("transfer", ""));
                return $"transfer{lastIdNumber + 1}";
            }
        }
    }
}

