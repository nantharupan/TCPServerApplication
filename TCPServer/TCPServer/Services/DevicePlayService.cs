using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCPServer.Configuration;
using TCPServer.Model;

namespace TCPServer.Services
{
    public class DevicePlayService : IDevicePlayService
    {
        private readonly IMongoCollection<DeviceLocation> _deviceLocation;
        private readonly TenantConfiguration _appSettings;


        public DevicePlayService(IOptions<TenantConfiguration> appSettings)
        {
          

            MongoClient dbClient = new MongoClient("mongodb+srv://User:User123@cluster0.8hake.mongodb.net/GPSTracking?retryWrites=true&w=majority");

            var dbList = dbClient.ListDatabases().ToList();

        
            var database = dbClient.GetDatabase("GPSTracking");

            _deviceLocation = database.GetCollection<DeviceLocation>(appSettings.Value.CollectionName);
            



        }
        public string AddDevicePlay(Byte[] devicelocationPacket)
        {
            var returnmessage = "78780501";
            var completeMessage = "";
            var OriginalMessage = "";
            var IEMI = "";
            var serialNo = "";
            byte[] serialNumber = null;
            try
            {
                Console.WriteLine("Message Recieved");
                //var message = Encoding.Unicode.GetString(devicelocationPacket);
                int messageLength = (int)devicelocationPacket[2]; 
                var protocol = devicelocationPacket[3].ToString("X2");
                

                serialNumber = devicelocationPacket.Skip(2 + 1 + messageLength - 4).Take(2).ToArray();


                if (protocol == "01")
                {
                    IEMI = devicelocationPacket[4].ToString("X2") + devicelocationPacket[5].ToString("X2") + devicelocationPacket[6].ToString("X2")
                + devicelocationPacket[7].ToString("X2") + devicelocationPacket[8].ToString("X2") + devicelocationPacket[9].ToString("X2")
                + devicelocationPacket[10].ToString("X2") + devicelocationPacket[11].ToString("X2");

                }
                else
                {
                    var SerialNo = serialNumber[0].ToString("X2") + serialNumber[1].ToString("X2");
                    returnmessage = returnmessage + SerialNo;
                }



                completeMessage = devicelocationPacket[1].ToString("X2") + devicelocationPacket[2].ToString("X2") + devicelocationPacket[3].ToString("X2")
                    + devicelocationPacket[4].ToString("X2") + devicelocationPacket[5].ToString("X2") + devicelocationPacket[6].ToString("X2")
                    + devicelocationPacket[7].ToString("X2") + devicelocationPacket[8].ToString("X2") + devicelocationPacket[9].ToString("X2")
                    + devicelocationPacket[10].ToString("X2") + devicelocationPacket[11].ToString("X2") + devicelocationPacket[12].ToString("X2")
                    + devicelocationPacket[13].ToString("X2") + devicelocationPacket[14].ToString("X2") + devicelocationPacket[15].ToString("X2")
                    + devicelocationPacket[16].ToString("X2") + devicelocationPacket[17].ToString("X2") + devicelocationPacket[18].ToString("X2");

                OriginalMessage = "52";

                string IMEI = Encoding.ASCII.GetString(devicelocationPacket.Skip(4).Take(messageLength - 5).ToArray());
                OriginalMessage = "55";
                int year = devicelocationPacket[4];
                OriginalMessage = "57";
                int month = devicelocationPacket[5];
                OriginalMessage = "59";
                int day = devicelocationPacket[6];
                OriginalMessage = "61";
                int hour = devicelocationPacket[7];
                OriginalMessage = "63";
                int minute = devicelocationPacket[8];
                OriginalMessage = "65";
                int second = devicelocationPacket[9];
                OriginalMessage = "67";
                var date = DateTime.Now;
                try
                {
                    date = new DateTime(2000+year, month, day, hour, minute, second);
                }
                catch (Exception )
                {

                }

                var ACStatus = devicelocationPacket[27].ToString("X2");
                OriginalMessage = "79";


                decimal speed = devicelocationPacket[19];
                OriginalMessage = "83";

                var lattitude = new byte[4];
                Array.Copy(devicelocationPacket, 11, lattitude, 0, 4);

                var number = BitConverter.ToUInt32(lattitude.Reverse().ToArray(), 0);
                var lat = (90 * number) / 162000000.0;
                OriginalMessage = "90";

                var longitude = new byte[4];
                Array.Copy(devicelocationPacket, 15, longitude, 0, 4);

                number = BitConverter.ToUInt32(longitude.Reverse().ToArray(), 0);
                var lon = (90 * number) / 162000000.0;
                OriginalMessage = "97";

                
                OriginalMessage = "100";


                OriginalMessage = "107";

              

                OriginalMessage = "108";

                var device = new DeviceLocation
                {
                    OriginalPacket = devicelocationPacket,
                    PacketLength = messageLength,
                    SerialNo = serialNumber[0].ToString("X2") + serialNumber[1].ToString("X2"),
                    GpsTime = date,
                    ISTGpsTime = DateTime.UtcNow,
                    Speed = speed,
                    Latitude = (decimal)lat,
                    Longitude = (decimal)lon,
                    ACC = ACStatus,
                    Protocol = completeMessage,
                    IEMI = IEMI
                    
                };
                _deviceLocation.InsertOne(device);
            }
            catch (Exception ex)
            {

                var device = new DeviceLocation
                {
                    OriginalPacket = devicelocationPacket,
                    GpsTime = DateTime.Now,
                    Protocol = completeMessage,
                    Exception = ex.Message,
                    OriginalMessage = OriginalMessage

                };
                _deviceLocation.InsertOne(device);
            }
           
           
            //Console.WriteLine(message);
            return re;
        }
    }
}
