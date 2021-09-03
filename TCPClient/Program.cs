using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace TCPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("TCP Client Starting…..");

            while (true)

            {

                var message = Console.ReadLine();

                //var bytes = System.Text.Encoding.Unicode.GetBytes(message);

                var bytes = StrToByteArray(message);

                SendMessage(bytes);

            }

        }



        private static byte[] SendMessage(byte[] messageBytes)

        {

            const int bytesize = 1024 * 1024;

            try

            {
                System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient("54.254.72.244", 3077);
                //System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient("18.119.28.184", 3077);
                //System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient("127.0.0.1", 3077);

                


                NetworkStream stream = client.GetStream();

                

                stream.Write(messageBytes, 0, messageBytes.Length);

                Console.WriteLine("Connected to server.");

                messageBytes = new byte[bytesize];

                // Receive the stream of bytes

                stream.Read(messageBytes, 0, messageBytes.Length);

                var messageToPrint = System.Text.Encoding.Unicode.GetString(messageBytes);

                Console.WriteLine(messageToPrint);

                stream.Dispose();

                client.Close();

            }

            catch (Exception ex)

            {

                Console.WriteLine(ex.Message);

            }
            return messageBytes; // Return response
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