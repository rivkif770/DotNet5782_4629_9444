using IDAL.DO;
using System;
using System.Runtime.Serialization;

namespace DalObject
{
    [Serializable]
    internal class PackageException : Exception
    {
        private string v;
        private Severity mild;

        public PackageException()
        {
        }

        public PackageException(string message) : base(message)
        {
        }

        public PackageException(string v, Severity mild)
        {
            this.v = v;
            this.mild = mild;
        }

        public PackageException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PackageException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}