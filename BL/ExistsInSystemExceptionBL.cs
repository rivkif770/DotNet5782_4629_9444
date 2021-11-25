using IDAL.DO;
using System;
using System.Runtime.Serialization;
/// <summary>
/// Anomalies if the object already exists
/// </summary>
namespace IBL.BO
{
    [Serializable]
    public class ExistsInSystemExceptionBL : Exception
    {
        private Severity mild;
        private string v;
        public ExistsInSystemExceptionBL()
        {
        }

        public ExistsInSystemExceptionBL(string message) : base(message)
        {
        }

        public ExistsInSystemExceptionBL(string v, Severity mild)
        {
            this.v = v;
            this.mild = mild;
        }

        public ExistsInSystemExceptionBL(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExistsInSystemExceptionBL(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}