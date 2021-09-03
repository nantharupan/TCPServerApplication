using System;
using System.Collections.Generic;
using System.Text;

namespace TCPServer.Configuration
{
    public class TenantConfiguration 
    {
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
