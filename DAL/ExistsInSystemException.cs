using DO;
using System;
using System.Runtime.Serialization;

namespace DO
{
    [Serializable]
    public class ExistsInSystemException : Exception
    {
        private string v;
        private Severity mild;

        public ExistsInSystemException()
        {
        }

        public ExistsInSystemException(string message, int uniqueID) : base(message)
        {
        }

        public ExistsInSystemException(string v, Severity mild)
        {
            this.v = v;
            this.mild = mild;
        }

        public ExistsInSystemException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExistsInSystemException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        [Serializable]
        internal class XMLFileLoadCreateException : Exception
        {
            private string filePath;
            private string v;
            private Exception ex;

            public XMLFileLoadCreateException()
            {
            }

            public XMLFileLoadCreateException(string message) : base(message)
            {
            }

            public XMLFileLoadCreateException(string message, Exception innerException) : base(message, innerException)
            {
            }

            public XMLFileLoadCreateException(string filePath, string v, Exception ex)
            {
                this.filePath = filePath;
                this.v = v;
                this.ex = ex;
            }

            protected XMLFileLoadCreateException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
    }
}