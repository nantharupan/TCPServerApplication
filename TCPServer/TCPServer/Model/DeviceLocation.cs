using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace TCPServer.Model
{
    public class DeviceLocation
    {
        public DeviceLocation()
        {
            this.Id = ObjectId.GenerateNewId().ToString();
        }


        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string TenantId { get; set; }
        public string IEMI { get; set; }
        public Decimal Longitude { get; set; }
        public Decimal Latitude { get; set; }
        public DateTime GpsTime { get; set; }
        public DateTime ISTGpsTime { get; set; }
        public long UnixGpsTime { get; set; }
        public Decimal Speed { get; set; }
        public Decimal Course { get; set; }
        public bool HandPlotted { get; set; }

        public string OriginalMessage { get; set; }

        public Byte[] OriginalPacket { get; set; }
        public int PacketLength { get; set; }

        public string SerialNo { get; set; }

        public string ACC { get; set; }

        public string Protocol { get; set; }
        public string Exception { get; set; }
    }
}
