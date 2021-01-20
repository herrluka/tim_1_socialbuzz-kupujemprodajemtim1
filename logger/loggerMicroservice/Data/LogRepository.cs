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

       /// <summary>
       /// Unos novog reda u tabelu Log u bazi
       /// </summary>
       /// <param name="record">log koji se unosi</param>
       /// <returns></returns>
        public Log InsertLog(Log record)
        {
            var collection = database.GetCollection<Log>("Log");
            collection.InsertOne(record);
            var returnCollection= collection.Find(Builders<Log>.Filter.Eq("ID", new Guid())).ToList();
            if (returnCollection.Count == 0)
                return null;
            return collection.Find(Builders<Log>.Filter.Eq("ID", new Guid())).First();
        }
        /// <summary>
        /// Metoda koja vraca sve logove iz baze po zadatim parametrima
        /// </summary>
        /// <param name="microserviceName">Naziv mikroservisa</param>
        /// <param name="logLevel">Nivo logovanja</param>
        /// <param name="from">Datum i vreme koji oznacavaju pocetak intervala za koji se vracaju logovi</param>
        /// <param name="to">Datum i vreme koji oznacavaju kraj intervala za koji se vracaju logovi</param>
        /// <returns></returns>
        public IList<Log> GetLogs(string microserviceName, string logLevel, DateTime from, DateTime to)
        {
            logLevel = (logLevel != null ?logLevel: "");
            microserviceName = (microserviceName != null ? microserviceName : "");
            if (to >= from)
            {
                to = DateTime.Now;
            }
            var collection = database.GetCollection<Log>("Log").Find(new BsonDocument()).ToList();
            return collection.FindAll((element) => (DateTime.Parse(element.TimeOfAction) > from && DateTime.Parse(element.TimeOfAction) < to && element.Microservice.Contains(microserviceName) && element.LogLevel.Contains(logLevel)));
        }
    }
}
