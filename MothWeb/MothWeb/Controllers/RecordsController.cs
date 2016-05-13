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

        RecordEvent[] testRecords = new RecordEvent[]
        {
            new RecordEvent{Date = new DateTime(2016,05,04)},
            new RecordEvent{Date = new DateTime(2016,05,05)},
            new RecordEvent{Date = new DateTime(2016,04,01),RecordType = "one type"},
            new RecordEvent{Date = new DateTime(2016,04,01),RecordType="another type same date"}
        
        };

        public IEnumerable<string> GetAllDates()
        {

            List<string> availableDates = new List<string>();
            mong = new MongoClient();
            mongdb = mong.GetDatabase("test2");

            var coll = mongdb.GetCollection<BsonDocument>("records");
            var filter = new BsonDocument();

            var result = coll.Find(filter).ToList();

            foreach (var item in result)
            {
                string raw = item.GetElement("Date").ToString();
                //date as string in YYYY-MM-DD format
                //string date = raw.Substring(5);
                string date = raw.Substring(5,20);
                availableDates.Add(date);
                
                /*DateTime dt;

                try
                {
                    dt = Convert.ToDateTime(date);
                    records.Add(new RecordEvent { Date = dt });
                }
                catch (FormatException)
                {
                    Console.WriteLine("ERROR: Couldn't convert {0} to a DateTime", date);
                    return null;
                }*/

            }

            Console.WriteLine("Found {0} records", result.Count);

            return availableDates;
        }

        //note paramter name has to match name in routing config (i.e. 'date'...)
        public IEnumerable<RecordEvent> GetRecord(string date, string fullDate)
        //public string GetRecord(string id)
        {
            List<RecordEvent> recsForDate = getRecordsForDate(fullDate);            
            //return testRecords;
            return recsForDate;
        }

        [NonAction]
        private List<RecordEvent> getRecordsForDate(string date)
        {            
            mong = new MongoClient();
            mongdb = mong.GetDatabase("test2");

            var coll = mongdb.GetCollection<BsonDocument>("records");
            var date1 = Convert.ToDateTime(date);            
            var date2 = date1.AddDays(1.0);//to catch all dates between yyyymmdd:00:00 and yyymmdd:23:59


            //http://stackoverflow.com/questions/33492242/mongodb-c-sharp-filter-by-datetime-property
            var filter = Builders<BsonDocument>.Filter.Gte(new StringFieldDefinition<BsonDocument, BsonDateTime>("Date"), new BsonDateTime(date1)) & Builders<BsonDocument>.Filter.Lt(new StringFieldDefinition<BsonDocument, BsonDateTime>("Date"), new BsonDateTime(date2)); 
            //var filter = Builders<BsonDocument>.Filter.Gt("Date", date);

            var result = coll.Find(filter).ToList();

            List<RecordEvent> recsForDate = new List<RecordEvent>();

            foreach(var item in result) {
                RecordEvent rec = new RecordEvent();
                rec.Records = new List<TaxonRecord>();

                string raw = item.GetElement("Date").ToString();
                DateTime dt = Convert.ToDateTime(raw.Substring(5, 20));
                rec.Date=dt;

                raw = item.GetElement("Type").ToString();
                rec.RecordType = raw;

                raw = item.GetElement("Grid Ref").ToString();
                rec.GridRef = raw;
                                
                try{
                    raw = item.GetElement("Notes").ToString();
                    rec.Notes = raw;
                }
                catch(KeyNotFoundException)
                {
                    rec.Notes="";
                }

                
                try
                {
                    var recs = item["records"].AsBsonArray;
                    foreach (var entry in recs)
                    {
                        TaxonRecord spec = new TaxonRecord();                        

                        try 
                        {                            
                            raw = entry["Vernacular Name"].ToString();
                            spec.CommonName = raw;
                            
                        }
                        catch(KeyNotFoundException)
                        {
                            spec.CommonName="";
                        }
                        try 
                        {
                            raw = entry["Order"].ToString();
                            spec.Order = raw;
                        }
                        catch(KeyNotFoundException)
                        {
                            spec.Order = "";
                        }
                        try
                        {
                            raw = entry["Family"].ToString();
                            spec.Family = raw;
                        }
                        catch (KeyNotFoundException)
                        {
                            spec.Family = "";
                        }
                        try
                        {
                            raw = entry["Subfamily"].ToString();
                            spec.SubFamily = raw;
                        }
                        catch (KeyNotFoundException)
                        {
                            spec.SubFamily = "";
                        }
                        try
                        {
                            raw = entry["Genus"].ToString();
                            spec.Genus = raw;
                        }
                        catch (KeyNotFoundException)
                        {
                            spec.Genus = "";
                        }
                        try
                        {
                            raw = entry["Species"].ToString();
                            spec.Species = raw;
                        }
                        catch (KeyNotFoundException)
                        {
                            spec.Species = "";
                        }
                        try
                        {
                            raw = entry["Subspecies"].ToString();
                            spec.SubSpecies = raw;
                        }
                        catch (KeyNotFoundException)
                        {
                            spec.SubSpecies = "";
                        }
                        try
                        {
                            raw = entry["Aberration"].ToString();
                            spec.Aberration = raw;
                        }
                        catch (KeyNotFoundException)
                        {
                            spec.Aberration = "";
                        }
                        try
                        {
                            raw = entry["Form"].ToString();
                            spec.Form = raw;
                        }
                        catch (KeyNotFoundException)
                        {
                            spec.Form = "";
                        }
                        try
                        {
                            raw = entry["Count"].ToString();
                            spec.count = raw;
                        }
                        catch (KeyNotFoundException)
                        {
                            spec.count = "";
                        }
                        try
                        {
                            raw = entry["Notes"].ToString();
                            spec.notes = raw;
                        }
                        catch (KeyNotFoundException)
                        {
                            spec.notes = "";
                        }



                        rec.Records.Add(spec);

                    }

                }
                catch(KeyNotFoundException)
                {

                }
                  

                recsForDate.Add(rec);
            }

            return recsForDate;
            //return testRecords.ToList();
        }
    }
}
