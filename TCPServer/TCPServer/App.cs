using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TCPServer.Configuration;
using TCPServer.Services;

namespace TCPServer
{
    public class App
    {
        private readonly ILogger<App> _logger;
        private readonly TenantConfiguration _appSettings;
        private readonly IDevicePlayService _devicePlayService;

        public App(IOptions<TenantConfiguration> appSettings, ILogger<App> logger, IDevicePlayService devicePlayService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
            _devicePlayService = devicePlayService;
        }

        public async Task Run(string[] args)
        {
            
            _logger.LogInformation("Starting...");

            Console.WriteLine("Hello world!");
            Console.WriteLine(_appSettings.CollectionName);

            _logger.LogInformation("Finished!");

            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 3077);

            TcpListener server = new TcpListener(ep);

            server.Start(); // start the server

            Console.WriteLine($"TCP server in docker started with ip: { ep.Address}, port: { ep.Port}. Waiting for connection…..");

            while (true)

            {
                const int size = 1024 * 1024;

                string message = null;

                byte[] buffer = new byte[size];

                var sender = server.AcceptTcpClient();

                sender.GetStream().Read(buffer, 0, size);

                // Read the message

                var returnmessage = _devicePlayService.AddDevicePlay(buffer);

                message = Encoding.Unicode.GetString(buffer);

                Console.WriteLine($"Message received: { message}");              


                byte[] bytes = StrToByteArray(returnmessage);

                sender.GetStream().Write(bytes, 0, bytes.Length); // Send the reply                

            }


            await Task.CompletedTask;
        }
        public static byte[] StrToByteArray(string str)
        {
            Dictionary<string, byte> hexindex = new Dictionary<string, byte>();
            for (int i = 0; i <= 255; i++)
                hexindex.Add(i.ToString("X2"), (byte)i);

            List<byte> hexres = new List<byte>();
            for (int i = 0; i < str.Length; i += 2)
                hexres.Add(hexindex[str.Substring(i, 2)]);

            return hexres.ToArray();
        }
    }


}
