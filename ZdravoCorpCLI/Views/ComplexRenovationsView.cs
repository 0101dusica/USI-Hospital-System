using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZdravoCorp.FacilitiesManagement.Renovations.Repository;
using ZdravoCorp.FacilitiesManagement.Renovations.Model;
using ZdravoCorp.FacilitiesManagement.Renovations.Service;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Service;
using ZdravoCorp.SchedulingManagement;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Repository;
using ZdravoCorp.FacilitiesManagement.Equipments.Repository;
using ZdravoCorp.FacilitiesManagement.Equipments.Model;

namespace ZdravoCorpCLI.Views
{
    public class ComplexRenovationsView
    {
        private RenovationService _renovationService { get; set; }
        private RoomService _roomService { get; set; }

        public ComplexRenovationsView()
        {
            _renovationService = new RenovationService(new RenovationRepository(new Serializer<ComplexRenovation>()), new RoomRepository(new Serializer<Room>()), new AppointmentRepository(new Serializer<Appointment>()));
            _roomService = new RoomService(new RoomRepository(new Serializer<Room>()), new RenovationRepository(new Serializer<ComplexRenovation>()), new EquipmentRepository(new Serializer<Equipment>()));


            ShowMenu();
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Options:");
                Console.WriteLine("1. Devide One Room Into Two");
                Console.WriteLine("2. Merge Two Rooms Into One");
                Console.WriteLine("3. Exit");

                Console.WriteLine();
                Console.Write("Enter option: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        DevideRoom();
                        break;
                    case "2":
                        MergeRooms();
                        break;
                    case "3":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }

            }
        }

        public string ChooseRooms()
        {
            Console.WriteLine();
            Console.WriteLine("--------------");
            Console.WriteLine("|    Id      |");
            Console.WriteLine("--------------");

            foreach (var roomId in _roomService.GetExistingRooms())
                Console.WriteLine($"| {roomId} |");

            Console.WriteLine("-------------------");

            Console.WriteLine("Choose Rooms:");
            string roomIdPick = Console.ReadLine();

            if (!_roomService.GetExistingRooms().Contains(roomIdPick))
            {
                Console.WriteLine("This room doesn't exist!");
                return "";
            }

            return roomIdPick;
        }

        public void DevideRoom()
        {
            string fromRoomId = ChooseRooms();
            string toRoomId = fromRoomId + "-2";

            DateTime fromTime, toTime;
            TimeSlot timeSlot;

            while (true)
            {
                Console.WriteLine("Input Start Date:");
                fromTime = CheckDateInput();

                Console.WriteLine("Input End Date:");
                toTime = CheckDateInput();

                timeSlot = new TimeSlot(fromTime, toTime);

                if (!_renovationService.IsRoomAvailableForRenovation(fromRoomId, timeSlot))
                    break;

                Console.WriteLine("This room is not available for renovation during the specified dates.");
            }

            List<string> roomIds = new List<string> { fromRoomId, toRoomId };

            ComplexRenovation renovation = new ComplexRenovation(_renovationService.NextId(), roomIds, timeSlot, RenovationType.Separation);
            _renovationService.Add(renovation);
            _renovationService.SetTimers(renovation);

            Console.WriteLine("You have successfully scheduled a complex renovation!");
        }

        public void MergeRooms()
        {
            string fromRoomId = ChooseRooms();
            string toRoomId;

            do
            {
                Console.WriteLine("You can't merge the same room!");
                toRoomId = ChooseRooms();
            } while (fromRoomId == toRoomId);

            DateTime fromTime, toTime;
            TimeSlot timeSlot;

            while (true)
            {
                Console.WriteLine("Input Start Date:");
                fromTime = CheckDateInput();

                Console.WriteLine("Input End Date:");
                toTime = CheckDateInput();

                timeSlot = new TimeSlot(fromTime, toTime);

                if (!_renovationService.IsRoomAvailableForRenovation(toRoomId, timeSlot))
                    break;

                Console.WriteLine("This room is not available for renovation during the specified dates.");
            }

            List<string> roomIds = new List<string> { fromRoomId, toRoomId };

            ComplexRenovation renovation = new ComplexRenovation(_renovationService.NextId(), roomIds, timeSlot, RenovationType.Merging);
            _renovationService.Add(renovation);
            _renovationService.SetTimers(renovation);

            Console.WriteLine("You have successfully scheduled a complex renovation!");
        }



        public DateTime CheckDateInput()
        {
            DateTime dateTime;
            bool isValidDateTime = false;

            while (!isValidDateTime)
            {
                Console.WriteLine("Input the date and time (format: yyyy-MM-dd):");
                string input = Console.ReadLine();

                // Specify the expected date and time format
                string format = "yyyy-MM-dd";

                if (DateTime.TryParseExact(input, format, null, System.Globalization.DateTimeStyles.None, out dateTime))
                {
                    isValidDateTime = true;
                }
                else
                {
                    Console.WriteLine("Invalid date and time format. Please try again.");
                }
            }

            return dateTime;
        }

    }
}
