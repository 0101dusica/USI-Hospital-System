using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.SchedulingManagement;

namespace ZdravoCorp.PatientHealthCareManagement.Referrals
{
    public abstract class Referral
    {
        public string Id { get; set; }
        public string FromDoctor { get; set; }
        public string PatientUsername { get; set; }

        protected Referral(string id, string patientUsername, string fromDoctor)
        {
            PatientUsername = patientUsername;
            Id = id;
            FromDoctor = fromDoctor;
        }

    }
}
