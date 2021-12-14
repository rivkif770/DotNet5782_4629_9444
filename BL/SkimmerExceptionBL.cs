using DO;
using System;
using System.Runtime.Serialization;

namespace BO
{
    [Serializable]
    public class SkimmerExceptionBL : Exception
    {
        private string v;
        private Severity mild;

        public SkimmerExceptionBL()
        {
        }

        public SkimmerExceptionBL(string message) : base(message)
        {
        }

        public SkimmerExceptionBL(string v, Severity mild)
        {
            this.v = v;
            this.mild = mild;
        }

        public SkimmerExceptionBL(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SkimmerExceptionBL(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}