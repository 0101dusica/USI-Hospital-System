using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.SchedulingManagement;

namespace ZdravoCorp.FacilitiesManagement.Renovations.Model
{
    
    public abstract class Renovation
    {
        public string Id { get; set; }
        public TimeSlot TimeSlot { get; set; }

        public Renovation() { }

        public Renovation(string id, TimeSlot timeSlot)
        {
            Id = id;
            TimeSlot = timeSlot;
        }


    }
}