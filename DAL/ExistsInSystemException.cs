using IDAL.DO;
using System;
using System.Runtime.Serialization;

namespace DalObject
{
    [Serializable]
    internal class ExistsInSystemException : Exception
    {
        private string v;
        private Severity mild;

        public ExistsInSystemException()
        {
        }

        public ExistsInSystemException(string message) : base(message)
        {
        }

        public ExistsInSystemException(string v, Severity mild)
        {
            this.v = v;
            this.mild = mild;
        }

        public ExistsInSystemException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExistsInSystemException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}