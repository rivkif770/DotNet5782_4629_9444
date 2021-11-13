//using IBL.BO;
using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    internal class BLSkimmerException : Exception
    {
        public BLSkimmerException()
        {
        }

        public BLSkimmerException(string message) : base(message)
        {
        }

        public BLSkimmerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BLSkimmerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}