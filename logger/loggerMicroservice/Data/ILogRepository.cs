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
        public IList<Log> GetLogs(string microserviceName, string logLevel, DateTime from, DateTime to);
    }
}
