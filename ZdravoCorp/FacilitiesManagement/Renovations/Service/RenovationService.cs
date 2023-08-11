using System;
using System.Collections.Generic;
using System.Windows;

using ZdravoCorp.FacilitiesManagement.Equipments.Model;
using ZdravoCorp.FacilitiesManagement.Renovations.Repository;
using ZdravoCorp.FacilitiesManagement.Renovations.Model;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Repository;
using ZdravoCorp.SchedulingManagement;

namespace ZdravoCorp.FacilitiesManagement.Renovations.Service
{
    public class RenovationService
    {
        private IRenovationRepository _renovationRepository;
        private IRoomRepository _roomRepository;
        private IAppointmentRepository _appointmentRepository;

        public RenovationService(IRenovationRepository renovationRepository, IRoomRepository roomRepository, IAppointmentRepository appointmentRepository)
        {
            _renovationRepository = renovationRepository;
            _roomRepository = roomRepository;
            _appointmentRepository = appointmentRepository;
        }

        public List<ComplexRenovation> GetAll()
        {
            return _renovationRepository.GetAll();
        }

        public ComplexRenovation? GetById(string id)
        {
            return _renovationRepository.GetById(id);
        }

        public void Add(ComplexRenovation renovation)
        {
            _renovationRepository.Add(renovation);
        }

        public void Delete(ComplexRenovation renovation)
        {
            _renovationRepository.Delete(renovation);
        }

        public void Update(ComplexRenovation updatedRenovation)
        {
            _renovationRepository.Update(updatedRenovation);
        }

        public string NextId()
        {
            return _renovationRepository.NextId();
        }

        public bool IsRoomAvaliableForRenovation(string roomId, TimeSlot timeSlot)
        {
            return _appointmentRepository.AreAppointmentsInTimeRangeForRoom(roomId, timeSlot);
        }

        public void SetTimers(ComplexRenovation renovation)
        {
            // Timer for changing rooms info
            TimeSpan startInterval = renovation.TimeSlot.StartTime > DateTime.Now ? renovation.TimeSlot.StartTime - DateTime.Now : TimeSpan.FromSeconds(1);

            System.Timers.Timer startRenovationTimer = new System.Timers.Timer(startInterval.TotalMilliseconds);
            startRenovationTimer.Elapsed += (sender, e) => { ChangeRenovationData(renovation.Id, renovation.RoomIds[0], renovation.TimeSlot); };

            startRenovationTimer.AutoReset = false;
            startRenovationTimer.Enabled = true;
            startRenovationTimer.Start();

            // Timer for ending reservation
            TimeSpan endInterval = renovation.TimeSlot.EndTime > DateTime.Now ? renovation.TimeSlot.EndTime - DateTime.Now : TimeSpan.FromSeconds(1);

            System.Timers.Timer endRenovationTimer = new System.Timers.Timer(endInterval.TotalMilliseconds);
            endRenovationTimer.Elapsed += (sender, e) => { IsRenovationDone(renovation.Id); };

            endRenovationTimer.AutoReset = false;
            endRenovationTimer.Enabled = true;
            endRenovationTimer.Start();
        }

        public void ChangeRenovationData(string renovationId, string roomId, TimeSlot timeSlot)
        {
            ComplexRenovation renovation = _renovationRepository.GetById(renovationId);
            Room primaryRoom = _roomRepository.GetById(roomId);

            if (renovation != null && primaryRoom != null)
            {
                switch (renovation.RenovationType)
                {
                    case RenovationType.Separation:
                        _roomRepository.UpdateStatus(roomId);

                        Room newRoom = new Room(roomId + "-2", primaryRoom.Capacity / 2, primaryRoom.Capacity / 2, primaryRoom.RoomType, primaryRoom.RoomStatus, new List<InventoryItem>());

                        primaryRoom.Capacity = primaryRoom.Capacity - newRoom.Capacity;

                        renovation.RoomIds.Add(roomId + "-2");
                        _renovationRepository.Update(renovation);

                        _roomRepository.Update(primaryRoom);
                        _roomRepository.Add(newRoom);

                        break;
                    case RenovationType.Merging:
                        Room secondaryRoom = _roomRepository.GetById(renovation.RoomIds[1]);

                        foreach (InventoryItem itemsSecondary in secondaryRoom.InventoryItems)
                        {
                            bool found = false;

                            foreach (InventoryItem itemsPrimary in primaryRoom.InventoryItems)
                            {
                                if (itemsPrimary.EquipmentId == itemsSecondary.EquipmentId)
                                {
                                    itemsPrimary.Quantity = itemsPrimary.Quantity + itemsSecondary.Quantity;
                                    found = true;
                                }
                            }

                            if (found == false)
                            {
                                primaryRoom.InventoryItems.Add(itemsSecondary);
                            }
                        }

                        primaryRoom.Capacity = primaryRoom.Capacity + secondaryRoom.Capacity;
                        primaryRoom.FreeBeds = primaryRoom.Capacity;

                        _roomRepository.Update(primaryRoom);
                        _roomRepository.Delete(secondaryRoom);

                        _roomRepository.UpdateStatus(primaryRoom.Id);

                        break;
                }
            }
        }

        public void IsRenovationDone(string renovationId)
        {
            ComplexRenovation renovation = _renovationRepository.GetById(renovationId);

            foreach (string roomId in renovation.RoomIds)
            {
                _roomRepository.UpdateStatus(roomId);
            }

        }
    }
}
