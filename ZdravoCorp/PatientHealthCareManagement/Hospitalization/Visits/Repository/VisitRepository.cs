using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PatientHealthCareManagement.Visits.Model;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PatientHealthCareManagement.Visits.Repository
{
    public class VisitRepository : IVisitRepository
    {
        private static List<Visit> _visits = new List<Visit>();
        private const string _storagePath = "../../../Data/Visits.json";

        private ISerializer<Visit> _serializer;

        public VisitRepository(ISerializer<Visit> serializer)
        {
            _serializer = serializer;
            _visits = Load();
        }

        public List<Visit> Load()
        {
            return _serializer.FromJson(_storagePath);
        }

        public void Save()
        {
            _serializer.ToJson(_storagePath, _visits);
        }

        public List<Visit> GetAll()
        {
            return _visits;
        }

        public Visit? GetById(string id)
        {
            return _visits.FirstOrDefault(v => v.Id.Equals(id));
        }

        public void Add(Visit visit)
        {
            _visits.Add(visit);
            Save();
        }

        public void Delete(Visit visit)
        {
            _visits.Remove(visit);
            Save();
        }


        public void Update(Visit visit)
        {
            var existingVisit = GetById(visit.Id);
            if (existingVisit != null)
            {
                existingVisit.Temperature = visit.Temperature;
                existingVisit.BloodPressure = visit.BloodPressure;
                existingVisit.Observations = visit.Observations;

                Save();
            }
        }
    }
}

