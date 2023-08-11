using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;


namespace ZdravoCorp.Utils.Serializer
{
    public class Serializer<T> : ISerializer<T>
    {
        public Serializer() { }

        public void ToJson(string fileName, List<T> objects)
        {
            string jsonInput = JsonConvert.SerializeObject(objects, Formatting.Indented);
            File.WriteAllText(fileName, jsonInput);
        }

        public List<T> FromJson(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            var jsonString = reader.ReadToEnd();
            var settings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ"
            };
            List<T>? objects = JsonConvert.DeserializeObject<List<T>>(jsonString, settings);
            if (objects == null) return new List<T>();


            return objects;
        }

    }
}
