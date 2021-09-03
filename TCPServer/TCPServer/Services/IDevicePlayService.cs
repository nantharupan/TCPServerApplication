using System;
using System.Collections.Generic;
using System.Text;
using TCPServer.Model;

namespace TCPServer.Services
{
    public interface IDevicePlayService
    {
        public string AddDevicePlay(Byte[] devicelocationPacket);
    }
}
