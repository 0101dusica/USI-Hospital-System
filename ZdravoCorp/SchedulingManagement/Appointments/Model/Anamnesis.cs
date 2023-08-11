using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.SchedulingManagement.Appointments.Model
{
    public class Anamnesis
    {
        public string Observations { get; set; }
        public List<string> Symptoms { get; set; }
        public Anamnesis()
        {
            Observations = string.Empty;
            Symptoms = new List<string>();
        }

        public Anamnesis(string observations, List<string> symptoms)
        {
            Observations = observations;
            Symptoms = symptoms;
        }


    }
}
