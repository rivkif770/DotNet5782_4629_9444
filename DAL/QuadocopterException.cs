using System;
using System.Runtime.Serialization;

namespace IDAL.DO
{
    [Serializable]
    public class QuadocopterException : Exception
    {
        Severity severity;
        public QuadocopterException()
        {
        }

        public QuadocopterException(string message, Severity severity) : base(message)
        {
            this.severity = severity;
        }

        public QuadocopterException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected QuadocopterException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}