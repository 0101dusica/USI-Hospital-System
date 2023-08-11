using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using ZdravoCorp.Utils.Command;
using ZdravoCorp.FacilitiesManagement.Renovations.RenovationSimple.ViewModel;
using ZdravoCorp.FacilitiesManagement.Renovations.RenovationComplex.View;
using ZdravoCorp.FacilitiesManagement.Renovations.RenovationComplex.ViewModel;
using ZdravoCorp.FacilitiesManagement.Renovations.Service;
using ZdravoCorp.FacilitiesManagement.Renovations.Model;
using ZdravoCorp.SchedulingManagement;
using ZdravoCorp.FacilitiesManagement.Renovations.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Model;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Repository;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Repository;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.FacilitiesManagement.Renovations.RenovationSimple.Command
{
    public class AddSimpleRenovationCommand : BaseCommand
    {
        private ScheduleRenovationViewModel _viewModel;
        private RenovationService _renovationService;

        public AddSimpleRenovationCommand(ScheduleRenovationViewModel viewModel)
        {
            _viewModel = viewModel;
            _renovationService = new RenovationService(new RenovationRepository(new Serializer<ComplexRenovation>()), new RoomRepository(new Serializer<Room>()), new AppointmentRepository(new Serializer<Appointment>()));
        }

        private bool CanExecute(object parameter)
        {
            TimeSpan renovationDuration = _viewModel.DateTo - _viewModel.DateFrom;
            TimeSlot timeSlot = new TimeSlot(_viewModel.DateFrom, _viewModel.DateTo);

            if (renovationDuration.TotalSeconds <= 0)
            {
                MessageBox.Show("You can't schedule renovation that will take 0 or less days!");
                return false;
            }
            else if (_renovationService.IsRoomAvaliableForRenovation(_viewModel.SelectedRenovationRoom, timeSlot) == true)
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
            TimeSpan renovationDuration = _viewModel.DateTo - _viewModel.DateFrom;
            TimeSlot timeSlot = new TimeSlot(_viewModel.DateFrom, _viewModel.DateTo);

            if (CanExecute(parameter))
            {
                List<string> roomIds = new List<string>();
                roomIds.Add(_viewModel.SelectedRenovationRoom);

                ComplexRenovation renovation = new ComplexRenovation(_renovationService.NextId(), roomIds, timeSlot, RenovationType.Separation);

                if (_viewModel.IsMergeRoomsChecked)
                {
                    renovation.RenovationType = RenovationType.Merging;
                    _renovationService.Add(renovation);

                    ComplexRenovationView complexRenovationView = new ComplexRenovationView();
                    complexRenovationView.DataContext = new ComplexRenovationViewModel(complexRenovationView, renovation);
                    complexRenovationView.ShowDialog();
                }
                else if (_viewModel.IsSeparateRoomsChecked || _viewModel.IsOneRoomChecked)
                {
                    if (_viewModel.IsSeparateRoomsChecked)
                    {
                        renovation.RenovationType = RenovationType.Separation;
                    }

                    _renovationService.Add(renovation);

                    _renovationService.SetTimers(renovation);

                    MessageBox.Show("You successfully Schedule Renovation!");
                }
                else
                {
                    MessageBox.Show("You need to check one of three checkBox!");
                }

            }
        }
    }
}
