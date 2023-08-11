using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.FacilitiesManagement.Equipments.Model;
using ZdravoCorp.FacilitiesManagement.Equipments.Repository;
using ZdravoCorp.FacilitiesManagement.Equipments.View;
using ZdravoCorp.FacilitiesManagement.Equipments.ViewModel;
using ZdravoCorp.FacilitiesManagement.Renovations.Model;
using ZdravoCorp.FacilitiesManagement.Renovations.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Model;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Service;
using ZdravoCorp.SchedulingManagement.Appointments.Examination.ViewModel;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Repository;
using ZdravoCorp.SchedulingManagement.Appointments.Service;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.SchedulingManagement.Appointments.Examination.Command
{
    public class EndExaminationCommand : BaseCommand
    {
        private AppointmentService _appointmentService;
        private RoomService _roomService;

        private DoctorExaminationViewModel _viewModel;

        public event EventHandler<bool> ExaminationEnded;

        public EndExaminationCommand(DoctorExaminationViewModel viewModel)
        {
            _viewModel = viewModel;
            _appointmentService = new AppointmentService(new AppointmentRepository(new Serializer<Appointment>()));
            _roomService = new RoomService(new RoomRepository(new Serializer<Room>()), new RenovationRepository(new Serializer<ComplexRenovation>()), new EquipmentRepository(new Serializer<Equipment>()));
        }

        private bool CanExecute(object parameter)
        {
            return _viewModel.Observation != null && _viewModel.Symptoms != null && _viewModel.SelectedRoom != null && _roomService.IsRoomAvaliableForAppointment(_viewModel.SelectedRoom.Id, DateTime.Now);
        }

        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                var appointment = _viewModel.Appointment;
                _appointmentService.UpdateExamination(appointment.Id, _viewModel.Observation, _viewModel.Symptoms.ToList(), _viewModel.SelectedRoom.Id);
                var view = new DoctorEquipmentUsageView();
                view.DataContext = new DoctorEquipmentUsageViewModel(_viewModel.SelectedRoom.Id);
                view.ShowDialog();

   
                _viewModel.View.Close();

               
                ExaminationEnded?.Invoke(this, true);
            }
            else
            {
                ExaminationEnded?.Invoke(this, false);
            }
            
        }
    }
}
