using System;
using System.Runtime.Serialization;

namespace IDAL.DO
{
    [Serializable]
    public class PackageException : Exception
    {
        Severity severity;
        public PackageException()
        {
        }

        public PackageException(string message, Severity severity) : base(message)
        {
            this.severity = severity;
        }

        public PackageException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PackageException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}