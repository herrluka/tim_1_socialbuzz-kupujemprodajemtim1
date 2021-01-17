using loggerMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace loggerMicroservice.Data
{
    public interface ILogRepository
    {
        public Log InsertLog(Log record);
        public IList<Log> GetLogsByMicroservice(string microserviceName);
        public IList<Log> GetLogsByInterval(DateTime from, DateTime to);
        public IList<Log> GetLogsByLogLevel(string logLevel);
    }
}
