using System.Collections.Generic;
using ZdravoCorp.FacilitiesManagement.Renovations.Model;

namespace ZdravoCorp.FacilitiesManagement.Renovations.Repository
{
    public interface IRenovationRepository
    {
        List<ComplexRenovation> GetAll();
        ComplexRenovation? GetById(string id);
        void Add(ComplexRenovation renovation);
        void Delete(ComplexRenovation renovation);
        void Update(ComplexRenovation updatedRenovation);
        string NextId();
    }
}
