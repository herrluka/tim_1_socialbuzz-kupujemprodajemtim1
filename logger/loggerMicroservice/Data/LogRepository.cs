using loggerMicroservice.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace loggerMicroservice.Data
{
    public class LogRepository : ILogRepository
    {
        public LogRepository(IConfiguration configuration)
        {
            Configuration = configuration;
            database = new MongoClient(Configuration["ConnectionStrings:MongoDB"]).GetDatabase("Log");
        }
        private IMongoDatabase database;

        public IConfiguration Configuration { get; }

       
        public Log InsertLog(Log record)
        {
            var collection = database.GetCollection<Log>("Log");
            collection.InsertOne(record);
            return collection.Find(Builders<Log>.Filter.Eq("ID", record.ID)).First();
        }

        public IList<Log> GetLogs(string microserviceName, string logLevel, DateTime from, DateTime to)
        {
            logLevel = (logLevel != null ?logLevel: "");
            microserviceName = (microserviceName != null ? microserviceName : "");
            if (to >= from)
            {
                to = DateTime.Now;
            }
            var collection = database.GetCollection<Log>("Log").Find(new BsonDocument()).ToList();
            string s = "sdsd";
            Console.WriteLine($"{s.Contains(logLevel)}");
            return collection.FindAll((element) => (DateTime.Parse(element.TimeOfAction) > from && DateTime.Parse(element.TimeOfAction) < to && element.Microservice.Contains(microserviceName) && element.LogLevel.Contains(logLevel)));
        }
    }
}
