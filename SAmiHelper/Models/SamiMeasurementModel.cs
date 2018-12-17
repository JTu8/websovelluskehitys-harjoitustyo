using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAmiHelper.Models
{
    public class SamiMeasurementModel
    {
        // measurement object: can be used to identify the measurement, not required
        public string Object { get; set; }

        // measurement tag: can be used to further identify the measurement, not required
        public string Tag { get; set; }

        // measurement time
        //public DateTimeOffset Timestamp { get; set; }

        // or use the ISO8601 formatted string for the measurement time.
        // If this is set then Timestamp property's value is ignored
        public DateTimeOffset TimestampISO8601 { get; set; }

        // a measurement can have a not, not required    
        public string Note { get; set; }

        // a measurement can have a location object, not required
        //public Location Location { get; set; }

        // a measurement must have some data!
        public List<SamiDataModel> Data { get; set; }
    }
}
