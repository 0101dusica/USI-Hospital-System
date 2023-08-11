using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.SchedulingManagement.Appointments.Service;
using ZdravoCorp.SchedulingManagement.FreeDays.Model;
using ZdravoCorp.SchedulingManagement.FreeDays.Repository;
using ZdravoCorp.SchedulingManagement.FreeDays.Service;
using ZdravoCorp.SchedulingManagement.FreeDays.ViewModel;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.SchedulingManagement.FreeDays.Commands
{
    public class AddFreeDaysCommand : BaseCommand
    {
        private FreeDaysService _freeDaysService;
        private AppointmentDoctorAvailabilityService _appointmentDoctorAvailabilityService;

        private AddFreeDaysViewModel _viewModel;

        public event EventHandler<bool> FreeDaysRequestAdded;


        public AddFreeDaysCommand(AddFreeDaysViewModel viewModel)
        {
            _viewModel = viewModel;
            _freeDaysService = new FreeDaysService(new FreeDaysRepository(new Serializer<FreeDay>()));
            _appointmentDoctorAvailabilityService = new AppointmentDoctorAvailabilityService();
        }

        private bool CanExecute(object parameter)
        {
            return _viewModel.Days > 0 && _viewModel.Description != null && _viewModel.SelectedDate > DateTime.Now.Date && _appointmentDoctorAvailabilityService.IsDoctorAvailabileForFreeDays(_viewModel.SelectedDate.Date, _viewModel.SelectedDate.AddDays(_viewModel.Days).Date, _viewModel.LoggedDoctor);
        }


        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                TimeSlot timeSlot = new TimeSlot(_viewModel.SelectedDate, _viewModel.SelectedDate.AddDays(_viewModel.Days));
                FreeDay freeDay = new FreeDay(_freeDaysService.NextId(), _viewModel.LoggedDoctor.Username, RequestStatus.InProccess, _viewModel.Description, timeSlot);
                _freeDaysService.Add(freeDay);
                FreeDaysRequestAdded?.Invoke(this, true);
            }
            else
            {
                FreeDaysRequestAdded?.Invoke(this, false);
            }
        }
    }
}
