using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

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

        public void RestoreState<T>()
        {
            stream.Seek(0, SeekOrigin.Begin);
            object o = new BinaryFormatter().Deserialize(stream);
            stream.Close();

            Mapper.CreateMap<T, T>();
            Mapper.Map((T)o, this);
        }
    }
}
