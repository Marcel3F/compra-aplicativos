using System;
using System.Runtime.Serialization;

namespace CompraAplicativos.Application.Exceptions
{
    [Serializable]
    public class MessageBrokerException : Exception
    {
        public MessageBrokerException()
        {
        }

        public MessageBrokerException(string message)
            : base(message)
        {
        }

        public MessageBrokerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected MessageBrokerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
