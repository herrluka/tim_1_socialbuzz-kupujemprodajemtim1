using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace loggerMicroservice.Models
{
    public record Log([Required]string LogLevel, string EventID, string RequestID, string PreviousRequestID, [Required]string Message, string ExceptionType, string ExceptionMessage,[Required]string TimeOfAction,[Required]string Microservice)
    {
        [BsonId]
        public Guid ID { get; set; }
    }
}
