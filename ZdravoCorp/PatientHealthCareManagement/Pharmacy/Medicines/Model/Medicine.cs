using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Model
{
    public class Medicine
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Ingredients { get; set; }

        public Medicine() { }
        public Medicine(string id, string name, List<string> ingredients)
        {
            Id = id;
            Name = name;
            Ingredients = ingredients;
        }


    }
}
