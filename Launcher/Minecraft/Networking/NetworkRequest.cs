using System;
using System.Text;
using System.Net.Sockets;

namespace Minecraft.Networking
{
    class NetworkRequest
    {
        private TcpClient client;
        private bool clientConnected;

        public NetworkRequest()
        {
            this.client = new TcpClient();

            try
            {
                client.Connect(Network.ADDRESS, Network.PORT);

                if (client.Connected)
                {
                    this.clientConnected = true;
                }
                else
                {
                    this.clientConnected = false;
                }
            }
            catch
            {
                this.clientConnected = false;
            }
        }

        public string Login(string username, string password, out bool error)
        {
            string returnValue = "";
            error = false;

            if (clientConnected)
            {
                Send(String.Format("LOGIN {0} {1}\n", username, password));
                returnValue = GetResponse();
            }
            else
            {
                returnValue = "Unable to connect to server.";
                error = true;
            }

            return returnValue;
        }

        public string Register(string username, string password, out bool error)
        {
            string returnValue = "";
            error = false;

            if (clientConnected)
            {
                Send(String.Format("REGISTER {0} {1}\n", username, password));
                returnValue = GetResponse();
            }
            else
            {
                returnValue = "Unable to connect to server.";
                error = true;
            }

            return returnValue;
        }

        public string GetResponse()
        {
            string returnValue = "";

            try
            {
                NetworkStream stream = client.GetStream();

                Byte[] bytes = new Byte[2048];

                int length;
                while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    var incommingData = new byte[length];
                    Array.Copy(bytes, 0, incommingData, 0, length);

                    returnValue = Encoding.ASCII.GetString(incommingData);
                    break;
                }
            }
            catch
            {
                if (returnValue == "")
                {
                    returnValue = "ERROR";
                }
            }

            client.Close();

            return returnValue;
        }

        public void Send(string command)
        {
            try
            {
                byte[] response = Encoding.ASCII.GetBytes(command);

                NetworkStream stream = client.GetStream();
                stream.Write(response, 0, response.Length);
            }
            catch
            {
                this.clientConnected = false;
            }
        }
    }
}
