
using LoggingClassLibrary;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReactionsService.FakeLogger
{
    public class FakeLoggerService : Logger
    {
        public FakeLoggerService(IConfiguration configuration) :
            base(configuration)
        {

        }
        public override void Log(LogLevel logLevel, string requestId, string previousRequestId, string message, Exception exception)
        {
            Task.Run(() =>
            {
                Thread.Sleep(500);
            });
        }
    }
}
