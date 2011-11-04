using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Integer.Infrastructure.DocumentModelling
{
    public class DenormalizedReference<T> : IEquatable<T>, IEquatable<DenormalizedReference<T>> where T : INamedDocument
    {
        private T reference;

        private string id;
        public string Id 
        {
            get 
            {
                if (reference != null)
                    return reference.Id;
                else
                    return id;
            }
            set 
            {
                id = value;
            } 
        }

        private string nome;
        public string Nome
        {
            get
            {
                if (reference != null)
                    return reference.Nome;
                else
                    return nome;
            }
            set
            {
                nome = value;
            }
        }

        public static implicit operator DenormalizedReference<T>(T doc)
        {
            return new DenormalizedReference<T>
            {
                reference = doc
            };
        }

        public bool Equals(T other)
        {
            if (other == null)
                return false;

            return this.Id == other.Id;
        }

        public bool Equals(DenormalizedReference<T> other)
        {
            if (other == null)
                return false;

            return this.Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            DenormalizedReference<T> other = (DenormalizedReference<T>)obj;
            if (other == null)
                return false;

            return this.Id == other.Id;
        }

       public override int GetHashCode()
        {
            return (this.Id.GetHashCode());
        }
    }
}
