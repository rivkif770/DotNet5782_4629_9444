using IDAL.DO;
using System;
using System.Runtime.Serialization;

namespace IDAL.DO
{
    [Serializable]
    public class IdDoesNotExistException : Exception
    {
        private string v;
        private Severity mild;

        public IdDoesNotExistException()
        {
        }

        public IdDoesNotExistException(string message) : base(message)
        {
        }

        public IdDoesNotExistException(string v, Severity mild)
        {
            this.v = v;
            this.mild = mild;
        }

        public IdDoesNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IdDoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}