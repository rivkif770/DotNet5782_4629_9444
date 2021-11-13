using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    internal class BLClientException : Exception
    {
        public BLClientException()
        {
        }

        public BLClientException(string message) : base(message)
        {
        }

        public BLClientException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BLClientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}