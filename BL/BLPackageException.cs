//using IBL.BO;
using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    internal class BLPackageException : Exception
    {
        public BLPackageException()
        {
        }

        public BLPackageException(string message) : base(message)
        {
        }

        public BLPackageException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BLPackageException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}