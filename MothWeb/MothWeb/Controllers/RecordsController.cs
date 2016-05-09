using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using MothWeb.Models;

using MongoDB.Bson;
using MongoDB.Driver;

namespace MothWeb.Controllers
{   
    public class RecordsController : ApiController
    {

        MongoClient mong;
        IMongoDatabase mongdb;

        List<RecordEvent> records = new List<RecordEvent>();

        /*RecordEvent[] records = new RecordEvent[]
        {
            new RecordEvent{Date = new DateTime(2016,05,04)},
            new RecordEvent{Date = new DateTime(2016,05,05)},
            new RecordEvent{Date = new DateTime(2016,04,01),RecordType = "one type"},
            new RecordEvent{Date = new DateTime(2016,04,01),RecordType="another type same date"}
        
        };*/

        public IEnumerable<RecordEvent> GetAllDates()
        {

            mong = new MongoClient();
            mongdb = mong.GetDatabase("test2");

            var coll = mongdb.GetCollection<BsonDocument>("records");
            var filter = new BsonDocument();
            
            var result = coll.Find(filter).ToList();

            foreach (var item in result)
            {
                string raw = item.GetElement("Date").ToString();
                string date=raw.Substring(5,raw.Length-6);
                                
                DateTime dt;

                try
                {
                    dt = Convert.ToDateTime(date);
                    records.Add(new RecordEvent { Date = dt });
                }
                catch (FormatException)
                {
                    Console.WriteLine("ERROR: Couldn't convert {0} to a DateTime", date);
                    return null;
                }
            }
                                    

            Console.WriteLine("Found {0} records", result.Count);

            return records;
        }
        public IHttpActionResult GetRecord(DateTime date)
        {
            return NotFound();
        }
    }
}
