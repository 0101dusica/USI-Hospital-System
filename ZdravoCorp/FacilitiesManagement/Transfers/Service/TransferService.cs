using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZdravoCorp.FacilitiesManagement.Transfers.Repository;
using ZdravoCorp.FacilitiesManagement.Transfers.Model;
using ZdravoCorp.FacilitiesManagement.Equipments.Model;

namespace ZdravoCorp.FacilitiesManagement.Transfers.Service
{
    public class TransferService
    {
        private ITransferRepository _transferRepository;

        public TransferService(ITransferRepository transferRepository)
        {
            _transferRepository = transferRepository;
        }

        public List<Transfer> GetAll()
        {
            return _transferRepository.GetAll();
        }

        public Transfer? GetById(string id)
        {
            return _transferRepository.GetById(id);
        }

        public void Add(Transfer transfer)
        {
            _transferRepository.Add(transfer);
        }

        public void Delete(Transfer transfer)
        {
            _transferRepository.Delete(transfer);
        }

        public void Update(Transfer updatedTransfer)
        {
            _transferRepository.Update(updatedTransfer);
        }

        public string NextId()
        {
            return _transferRepository.NextId();
        }

        public bool AddInventoryItem(string transferId, InventoryItem transferItem)
        {
            Transfer transfer = _transferRepository.GetById(transferId);

            var existingItem = transfer.InventoryItems.FirstOrDefault(item => item.EquipmentId == transferItem.EquipmentId);
            if (existingItem != null)
            {
                if (existingItem.Quantity < existingItem.Reserved + transferItem.Reserved)
                {
                    return false;
                }
                else
                {
                    existingItem.Reserved += transferItem.Reserved;
                }
            }
            else
            {
                transfer.InventoryItems.Add(transferItem);
            }

            _transferRepository.Update(transfer);
            return true;
        }


        //public void ChangeFileDeferred(MainStorage mainStorage)
        //{

        //    foreach (Transfer transfer in mainStorage.Transfers)
        //    {
        //        if (transfer.TransferType == TransferType.Deferred && transfer.TransferStatus == TransferStatus.InProcess)
        //        {
        //            TimeSpan timeSpan = DateTime.Now - transfer.TimeSlot.EndTime;
        //            if (timeSpan.TotalSeconds >= 0)
        //            {
        //                this.UpdateInventoryItems(mainStorage, transfer);
        //            }
        //        }
        //    }
        //}

        //public void UpdateInventoryItems(MainStorage MainStorage, Transfer transfer)
        //{

        //    foreach (InventoryItem itemTransfer in transfer.InventoryItems)
        //    {
        //        bool findItem = false;

        //        for (int i = 0; i < MainStorage.Hospitals.Count; i++)
        //        {
        //            foreach (Warehouse warehouse in MainStorage.Hospitals[i].Warehouses)
        //            {
        //                if (warehouse.Id == transfer.FromRoomId) //brisanje reserved-a
        //                {
        //                    foreach (InventoryItem itemWarehouse in warehouse.InventoryItems)
        //                    {
        //                        if (itemWarehouse.EquipmentId == itemTransfer.EquipmentId)
        //                        {
        //                            itemWarehouse.Reserved = itemWarehouse.Reserved - itemTransfer.Reserved;

        //                        }
        //                    }
        //                }

        //                if (warehouse.Id == transfer.ToRoomId) //dodavanje na quantity
        //                {
        //                    foreach (InventoryItem itemWarehouse in warehouse.InventoryItems)
        //                    {

        //                        if (itemWarehouse.EquipmentId == itemTransfer.EquipmentId)
        //                        {
        //                            findItem = true;
        //                            itemWarehouse.Quantity = itemWarehouse.Quantity + itemTransfer.Reserved;

        //                        }
        //                    }
        //                }
        //            }

        //            foreach (Room room in MainStorage.Hospitals[i].Rooms)
        //            {
        //                {
        //                    if (room.Id == transfer.FromRoomId) //brisanje reserved-a
        //                    {
        //                        foreach (InventoryItem itemWarehouse in room.InventoryItems)
        //                        {
        //                            if (itemWarehouse.EquipmentId == itemTransfer.EquipmentId)
        //                            {
        //                                itemWarehouse.Reserved = itemWarehouse.Reserved - itemTransfer.Reserved;
        //                            }
        //                        }
        //                    }

        //                    if (room.Id == transfer.ToRoomId) //dodavanje na quantity
        //                    {
        //                        foreach (InventoryItem itemWarehouse in room.InventoryItems)
        //                        {
        //                            if (itemWarehouse.EquipmentId == itemTransfer.EquipmentId)
        //                            {
        //                                findItem = true;
        //                                itemWarehouse.Quantity = itemWarehouse.Quantity + itemTransfer.Reserved;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        if (findItem == false)
        //        {
        //            InventoryItem newInventoryItem = new InventoryItem(itemTransfer.EquipmentId, itemTransfer.Reserved, 0);

        //            foreach (Warehouse warehouse in MainStorage.Hospitals[0].Warehouses)
        //            {
        //                if (warehouse.Id == transfer.ToRoomId)
        //                {
        //                    warehouse.InventoryItems.Add(newInventoryItem);
        //                }
        //            }

        //            foreach (Room room in MainStorage.Hospitals[0].Rooms)
        //            {
        //                if (room.Id == transfer.ToRoomId)
        //                {
        //                    room.InventoryItems.Add(newInventoryItem);
        //                }
        //            }
        //        }

        //    }

        //    foreach (Transfer tranferStorage in MainStorage.Transfers)
        //    {
        //        if (tranferStorage.Id == transfer.Id)
        //        {
        //            tranferStorage.TransferStatus = TransferStatus.Done;
        //        }
        //    }

        //    MainStorage.transferStorage.Save(MainStorage.Transfers);
        //    MainStorage.hospitalStorage.Save(MainStorage.Hospitals);
        //}
    }
}
