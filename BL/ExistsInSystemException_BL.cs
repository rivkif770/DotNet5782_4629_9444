using System;
using System.Runtime.Serialization;

namespace BlObject
{
    [Serializable]
    internal class ExistsInSystemException_BL : Exception
    {
        public ExistsInSystemException_BL()
        {
        }

        public ExistsInSystemException_BL(string message) : base(message)
        {
        }

        public ExistsInSystemException_BL(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExistsInSystemException_BL(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}