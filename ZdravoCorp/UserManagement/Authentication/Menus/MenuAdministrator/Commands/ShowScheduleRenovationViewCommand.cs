using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.FacilitiesManagement.Equipments.Model;
using ZdravoCorp.FacilitiesManagement.Equipments.Repository;
using ZdravoCorp.FacilitiesManagement.Renovations.Model;
using ZdravoCorp.FacilitiesManagement.Renovations.RenovationSimple.View;
using ZdravoCorp.FacilitiesManagement.Renovations.RenovationSimple.ViewModel;
using ZdravoCorp.FacilitiesManagement.Renovations.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Model;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Service;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.UserManagement.Authentication.Menus.MenuAdministrator.Commands
{
    public class ShowScheduleRenovationViewCommand : BaseCommand
    {
        private readonly RoomService _roomService;
        public ShowScheduleRenovationViewCommand()
        {
            _roomService = new RoomService(new RoomRepository(new Serializer<Room>()), new RenovationRepository(new Serializer<ComplexRenovation>()), new EquipmentRepository(new Serializer<Equipment>()));
        }

        public override void Execute(object? parameter)
        {
            ScheduleRenovationView scheduleRenovationView = new ScheduleRenovationView();
            scheduleRenovationView.DataContext = new ScheduleRenovationViewModel(scheduleRenovationView);
            scheduleRenovationView.ShowDialog();
        }
    }
}
