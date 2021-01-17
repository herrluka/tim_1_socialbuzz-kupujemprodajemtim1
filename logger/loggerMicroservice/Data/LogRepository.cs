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

        public IList<Log> GetLogsByInterval(DateTime from, DateTime to)
        {
            var collection = database.GetCollection<Log>("Log").Find(new BsonDocument()).ToList();
            return collection.FindAll((element) => (DateTime.Parse(element.TimeOfAction) > from && DateTime.Parse(element.TimeOfAction)<to));
        }

        public IList<Log> GetLogsByLogLevel(string logLevel)
        {
            var collection = database.GetCollection<Log>("Log").Find(new BsonDocument()).ToList();
            return collection.FindAll((element) => element.LogLevel == logLevel);
        }

        public IList<Log> GetLogsByMicroservice(string microserviceName)
        {
            var collection = database.GetCollection<Log>("Log").Find(new BsonDocument()).ToList();
            return collection.FindAll((element) => element.Microservice == microserviceName);
        }

        public Log InsertLog(Log record)
        {
            var collection = database.GetCollection<Log>("Log");
            collection.InsertOne(record);
            return collection.Find(Builders<Log>.Filter.Eq("ID", record.ID)).First();
        }
    }
}
