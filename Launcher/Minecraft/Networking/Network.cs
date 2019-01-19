using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Minecraft.Networking
{
    class Network
    {
        public static IPAddress ADDRESS = IPAddress.Parse("127.0.0.1");
        public static int PORT = 300;
    }
}
