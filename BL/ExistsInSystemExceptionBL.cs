using DO;
using System;
using System.Runtime.Serialization;
/// <summary>
/// Anomalies if the object already exists
/// </summary>
namespace BO
{
    [Serializable]
    public class ExistsInSystemExceptionBL : Exception
    {
        private Severity mild;
        private string v;
        public ExistsInSystemExceptionBL(string v)
        {
        }

        public ExistsInSystemExceptionBL(string message, DO.Severity mild) : base(message)
        {
        }


        public ExistsInSystemExceptionBL(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExistsInSystemExceptionBL(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}