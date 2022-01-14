using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    internal class LocationExceptionBL : Exception
    {
        public LocationExceptionBL()
        {
        }

        public LocationExceptionBL(string message) : base(message)
        {
        }

        public LocationExceptionBL(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LocationExceptionBL(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}