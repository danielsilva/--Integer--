using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading.Tasks;

namespace Integer.Infrastructure.Domain
{
    [Serializable]
    public abstract class Entity
    {
        MemoryStream stream = new MemoryStream();

        public void SaveState()
        {
            new BinaryFormatter().Serialize(stream, this);
        }

        public T RestoreState<T>()
        {
            stream.Seek(0, SeekOrigin.Begin);
            object o = new BinaryFormatter().Deserialize(stream);
            stream.Close();

            return (T)o;
        }

    }
}
