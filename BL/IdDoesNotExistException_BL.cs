using IDAL.DO;
using System;
using System.Runtime.Serialization;

namespace BlObject
{
    [Serializable]
    internal class IdDoesNotExistException_BL : Exception
    {
        private string v;
        private Severity mild;
        public IdDoesNotExistException_BL()
        {
        }

        public IdDoesNotExistException_BL(string message) : base(message)
        {
        }

        public IdDoesNotExistException_BL(string v, Severity mild)
        {
            this.v = v;
            this.mild = mild;
        }
        public IdDoesNotExistException_BL(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IdDoesNotExistException_BL(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}