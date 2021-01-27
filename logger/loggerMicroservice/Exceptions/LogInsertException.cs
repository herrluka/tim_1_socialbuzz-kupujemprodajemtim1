using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace loggerMicroservice.Exceptions
{
    public class LogInsertException : Exception
    {
        public LogInsertException()
        {
        }

        public LogInsertException(string message) : base(message)
        {
        }

        public LogInsertException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LogInsertException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
