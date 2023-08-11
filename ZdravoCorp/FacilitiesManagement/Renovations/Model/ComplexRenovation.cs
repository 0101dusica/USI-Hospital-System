using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.SchedulingManagement;

namespace ZdravoCorp.FacilitiesManagement.Renovations.Model
{
    public enum RenovationType
    {
        Separation,
        Merging
    }
    public class ComplexRenovation : Renovation
    {
        public RenovationType RenovationType { get; set; }

        public List<string> RoomIds;
        public ComplexRenovation() { }
        public ComplexRenovation(string id, List<string> roomIds, TimeSlot timeSlot, RenovationType renovationType) : base(id, timeSlot)
        {
            RoomIds = roomIds;
            RenovationType = renovationType;
        }
    }
}
