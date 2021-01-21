using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendation_Service.Data
{
    public interface ILoggerService : ILogger
    {
        public void Log(LogLevel logLevel, string requestId, string previousRequestId, string message, Exception exception);
    }
}
