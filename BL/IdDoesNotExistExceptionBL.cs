using DO;
using System;
using System.Runtime.Serialization;
/// <summary>
/// Anomalies if the object does not exist
/// </summary>
namespace BO
{
    [Serializable]
    public class IdDoesNotExistExceptionBL : Exception
    {
        private string v;
        private Severity mild;
        public IdDoesNotExistExceptionBL()
        {
        }

        public IdDoesNotExistExceptionBL(string message) : base(message)
        {
        }

        public IdDoesNotExistExceptionBL(string v, Severity mild)
        {
            this.v = v;
            this.mild = mild;
        }
        public IdDoesNotExistExceptionBL(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IdDoesNotExistExceptionBL(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}