using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ZdravoCorp.FacilitiesManagement.Renovations.Model;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.FacilitiesManagement.Renovations.Repository
{
    public class RenovationRepository : IRenovationRepository
    {
        private static List<ComplexRenovation> _renovations = new List<ComplexRenovation>();
        private const string _storagePath = "../../../../Data/Renovations.json";

        private ISerializer<ComplexRenovation> _serializer;

        public RenovationRepository(ISerializer<ComplexRenovation> serializer)
        {
            _serializer = serializer;
            _renovations = Load();
        }

        public List<ComplexRenovation> Load()
        {
            return _serializer.FromJson(_storagePath);
        }

        public void Save()
        {
            _serializer.ToJson(_storagePath, _renovations);
        }

        public List<ComplexRenovation> GetAll()
        {
            return _renovations;
        }

        public ComplexRenovation? GetById(string id)
        {
            return _renovations.FirstOrDefault(r => r.Id.Equals(id));
        }

        public void Add(ComplexRenovation renovation)
        {
            _renovations.Add(renovation);
            Save();
        }

        public void Delete(ComplexRenovation renovation)
        {
            _renovations.Remove(renovation);
            Save();
        }

        public void Update(ComplexRenovation updatedRenovation)
        {
            var existingRenovation = GetById(updatedRenovation.Id);
            if (existingRenovation != null)
            {
                existingRenovation.RoomIds = updatedRenovation.RoomIds;
                existingRenovation.RenovationType = updatedRenovation.RenovationType;
                existingRenovation.TimeSlot = updatedRenovation.TimeSlot;

                Save();
            }
        }

        public string NextId()
        {
            string? lastId = _renovations.LastOrDefault()?.Id;
            if (lastId == null)
            {
                return "renovation1";
            }
            else
            {
                int lastIdNumber = int.Parse(lastId.Replace("renovation", ""));
                return $"renovation{lastIdNumber + 1}";
            }
        }
    }
}
