using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAmiHelper.Models
{
    public class SamiDataModel
    {
        // data tag: Data must have a tag which uniquelly identifies the data or the sensor
        public string Tag { get; set; }

        // Value for the data, this is usually used to save the sensor value     
        public double? Value { get; set; }

        // a data can have other type values or they can be set to null or not provided at all
        // a data can have an integer value, not required
        public long? LongValue { get; set; }

        // a data can have a text value, not required
        public string TextValue { get; set; }

        // a data can have binary value, not required
        public byte[] BinaryValue { get; set; }

        // a data can have xml value, not required
        public string XmlValue { get; set; }

        // a data can have binary value in base64 encoded string format, not required
        // If this is set with a valid base64 encoded string then BinaryValue property is ignored.
        public string BinaryValueBase64 { get; set; }
    }
}
