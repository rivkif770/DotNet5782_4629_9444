using IDAL.DO;
using System;
using System.Runtime.Serialization;
/// <summary>
/// Anomalies if the object already exists
/// </summary>
namespace BlObject
{
    [Serializable]
    internal class ExistsInSystemException_BL : Exception
    {
        private Severity mild;
        private string v;
        public ExistsInSystemException_BL()
        {
        }

        public ExistsInSystemException_BL(string message) : base(message)
        {
        }

        public ExistsInSystemException_BL(string v, Severity mild)
        {
            this.v = v;
            this.mild = mild;
        }

        public ExistsInSystemException_BL(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExistsInSystemException_BL(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}