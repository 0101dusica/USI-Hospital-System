using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Timers;

using ZdravoCorp.Utils.Command;
using ZdravoCorp.FacilitiesManagement.Renovations.RenovationComplex.ViewModel;
using ZdravoCorp.FacilitiesManagement.Renovations.Service;
using ZdravoCorp.FacilitiesManagement.Renovations.Repository;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.FacilitiesManagement.Renovations.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Model;

namespace ZdravoCorp.FacilitiesManagement.Renovations.RenovationComplex.Command
{
    public class ShowComplexRenovationViewCommand : BaseCommand
    {
        private ComplexRenovationViewModel _viewModel;
        private RenovationService _renovationService;
        public ShowComplexRenovationViewCommand(ComplexRenovationViewModel viewModel)
        {
            _viewModel = viewModel;
            _renovationService = new RenovationService(new RenovationRepository(new Serializer<ComplexRenovation>()), new RoomRepository(new Serializer<Room>()), new AppointmentRepository(new Serializer<Appointment>()));
        }

        public bool CanExecute(object parameter)
        {
            if (_viewModel.SelectedConnectionRoom == _viewModel.Renovation.RoomIds[0])
            {
                MessageBox.Show("You can't merge one room!");
                return false;
            }
            else if (_renovationService.IsRoomAvaliableForRenovation(_viewModel.SelectedConnectionRoom, _viewModel.Renovation.TimeSlot) == true)
            {
                MessageBox.Show("This room is not avaliable for renovation in dates that you put!");
                return false;
            }
            else
            {
                return true;
            }
        }

        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                _viewModel.Renovation.RoomIds.Add(_viewModel.SelectedConnectionRoom);
                _renovationService.Update(_viewModel.Renovation);

                _renovationService.SetTimers(_viewModel.Renovation);

                _viewModel.ComplexRenovationView.Close();

                MessageBox.Show("You successfully Schedule Renovation!");
            }
        }
    }
}
