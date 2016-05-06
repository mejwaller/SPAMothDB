using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MothWeb.Models
{
    public class TaxonRecord
    {
        public string CommonName { get; set; }
        public string Order { get; set; }
        public string Family { get; set; }
        public string SubFamily { get; set; }
        public string Genus { get; set; }
        public string Species { get; set; }
        public string SubSpecies { get; set; }
        public string Aberration { get; set; }
        public string Form { get; set; }
        public int count { get; set; }
        public string notes { get; set; }

    }

    public class RecordEvent
    {
        public DateTime Date { get; set; }
        public string RecordType { get; set; }
        public string GridRef { get; set; }
        public string Notes { get; set; }
        public TaxonRecord[] Records { get; set; }

    }
}