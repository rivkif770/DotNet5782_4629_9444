using System;
using System.Runtime.Serialization;

namespace IDAL.DO
{
    [Serializable]
    public class BaseStationException : Exception
    {
        Severity severity;
        public BaseStationException()
        {
        }

        public BaseStationException(string message, Severity severity) : base(message)
        {
            this.severity = severity;
        }

        public BaseStationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BaseStationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}