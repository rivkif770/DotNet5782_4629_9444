using IDAL.DO;
using System;
using System.Runtime.Serialization;

namespace DalObject
{
    [Serializable]
    internal class QuadocopterException : Exception
    {
        private string v;
        private Severity mild;

        public QuadocopterException()
        {
        }

        public QuadocopterException(string message) : base(message)
        {
        }

        public QuadocopterException(string v, Severity mild)
        {
            this.v = v;
            this.mild = mild;
        }

        public QuadocopterException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected QuadocopterException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}