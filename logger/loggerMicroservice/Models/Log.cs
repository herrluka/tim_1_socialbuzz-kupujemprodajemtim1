using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace loggerMicroservice.Models
{
    public record Log(string LogLevel, string EventID, string RequestID, string PreviousRequestID, string Message, string ExceptionType, string ExceptionMessage,string TimeOfAction,string Microservice)
    {
        [BsonId]
        public Guid ID { get; set; }
    }
}
