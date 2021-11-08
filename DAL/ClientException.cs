using System;
using System.Runtime.Serialization;

namespace IDAL.DO
{
    [Serializable]
    public class ClientException : Exception
    {
        Severity severity;
        public ClientException()
        {
        }

        public ClientException(string message, Severity severity) : base(message)
        {
            this.severity = severity;
        }

        public ClientException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ClientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}