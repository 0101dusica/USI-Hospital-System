using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.FacilitiesManagement.Equipments.ViewModel;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.FacilitiesManagement.Equipments.Command
{
    public class ResetCheckBoxCommand : BaseCommand
    {
        EquipmentDisplayViewModel _equipmentDisplayViewModel;

        public ResetCheckBoxCommand(EquipmentDisplayViewModel equipmentDisplayViewModel)
        {
            _equipmentDisplayViewModel = equipmentDisplayViewModel;
        }

        public override void Execute(object? parameter)
        {
            _equipmentDisplayViewModel.IsOperatingRoomChecked = false;
            _equipmentDisplayViewModel.IsPatientRoomChecked = false;
            _equipmentDisplayViewModel.IsExaminationRoomChecked = false;
            _equipmentDisplayViewModel.IsWaitingRoomChecked = false;
            _equipmentDisplayViewModel.IsAppointmentsChecked = false;
            _equipmentDisplayViewModel.IsSurgeriesChecked = false;
            _equipmentDisplayViewModel.IsRoomFurnitureChecked = false;
            _equipmentDisplayViewModel.IsHallwayEquipmentsChecked = false;
            _equipmentDisplayViewModel.IsZeroChecked = false;
            _equipmentDisplayViewModel.IsLessThan10Checked = false;
            _equipmentDisplayViewModel.IsMoreThan10Checked = false;
            _equipmentDisplayViewModel.IsInsideChecked = false;
            _equipmentDisplayViewModel.IsOutsideChecked = false;
        }
    }
}
