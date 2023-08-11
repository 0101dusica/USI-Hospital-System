using Hangfire.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.SchedulingManagement;

namespace ZdravoCorp.FacilitiesManagement.Renovations.Model
{
    public class SimpleRenovation : Renovation
    {
        public string RoomId;
        public SimpleRenovation(string id, string roomId, TimeSlot timeSlot) : base(id, timeSlot)
        {
            RoomId = roomId;
        }
    }
}
