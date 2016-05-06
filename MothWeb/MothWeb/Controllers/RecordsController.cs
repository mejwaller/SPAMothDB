using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using MothWeb.Models;

namespace MothWeb.Controllers
{    
    public class RecordsController : ApiController
    {
        RecordEvent[] records = new RecordEvent[]
        {
            new RecordEvent{Date = new DateTime(2016,05,04)},
            new RecordEvent{Date = new DateTime(2016,05,05)},
            new RecordEvent{Date = new DateTime(2016,04,01),RecordType = "one type"},
            new RecordEvent{Date = new DateTime(2016,04,01),RecordType="another type same date"}
        
        };

        public IEnumerable<RecordEvent> GetAllDates()
        {
            return records;
        }
        public IHttpActionResult GetRecord(DateTime date)
        {
            return NotFound();
        }
    }
}
