using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Doctors.Repository;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Repository;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Service
{
    public class PatientDoctorAssignmentService
    {
        private readonly PatientService _patientService;
        private readonly DoctorService _doctorService;


        public PatientDoctorAssignmentService()
        {
            _patientService = new PatientService(new PatientRepository(new Serializer<Patient>()));
            _doctorService = new DoctorService(new DoctorRepository(new Serializer<Doctor>()));

        }

        public bool IsPatientAssignedToDoctor(string doctorUsername, string patientUsername)
        {
            Doctor doctor = _doctorService.GetByUsername(doctorUsername);
            Patient patient = _patientService.GetByUsername(patientUsername);

            return doctor.AppointmentIds.Intersect(patient.MedicalRecord.AppointmentIds).Any(); ;
        }
    }
}
