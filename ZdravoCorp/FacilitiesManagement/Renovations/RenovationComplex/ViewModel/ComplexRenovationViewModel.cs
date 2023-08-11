using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using ZdravoCorp.FacilitiesManagement.Renovations.RenovationComplex.Command;
using ZdravoCorp.FacilitiesManagement.Renovations.RenovationComplex.View;
using ZdravoCorp.FacilitiesManagement.Renovations.Model;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Service;

using ZdravoCorp.Utils.ViewModel;
using ZdravoCorp.FacilitiesManagement.Equipments.Model;
using ZdravoCorp.FacilitiesManagement.Equipments.Repository;
using ZdravoCorp.FacilitiesManagement.Renovations.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Model;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Repository;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.FacilitiesManagement.Renovations.RenovationComplex.ViewModel
{
    public class ComplexRenovationViewModel : BaseViewModel
    {
        public ComplexRenovation Renovation { get; set; }
        public ComplexRenovationView ComplexRenovationView { get; set; }

        public List<string> ConnectionRoom { get; set; }

        private string _selectedConnectionRoom;
        private ComplexRenovationView complexRenovationView;

        public string SelectedConnectionRoom
        {
            get { return _selectedConnectionRoom; }
            set { _selectedConnectionRoom = value; }
        }

        public ICommand SubmitButton { get; }

        public ComplexRenovationViewModel(ComplexRenovationView complexRenovationView, ComplexRenovation renovation)
        {
            Renovation = renovation;
            ComplexRenovationView = complexRenovationView;
            ConnectionRoom = new RoomService(new RoomRepository(new Serializer<Room>()), new RenovationRepository(new Serializer<ComplexRenovation>()), new EquipmentRepository(new Serializer<Equipment>())).GetExistingRooms();

            SubmitButton = new ShowComplexRenovationViewCommand(this);
        }
    }
}
