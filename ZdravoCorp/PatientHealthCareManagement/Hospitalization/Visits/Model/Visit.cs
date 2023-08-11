using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.PatientHealthCareManagement.Visits.Model
{
    public class Visit
    {
        public string Id { get; set; }
        public string BloodPressure { get; set; }
        public double Temperature { get; set; }
        public string Observations { get; set; }

        public Visit() { }
        public Visit(string id, string bloodPressure, double temperature, string observation)
        {
            Id = id;
            BloodPressure = bloodPressure;
            Temperature = temperature;
            Observations = observation;
        }

    }
}
