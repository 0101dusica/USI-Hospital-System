using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Utils.Serializer
{
    public interface ISerializer<T>
    {
        void ToJson(string fileName, List<T> objects);
        List<T> FromJson(string fileName);
    }
}
