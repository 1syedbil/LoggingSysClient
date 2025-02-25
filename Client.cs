using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Configuration;
using System.Data.SqlTypes;

namespace NetworkingA3Client
{
    internal class Client
    {

        private int serverPort = int.Parse(ConfigurationManager.AppSettings["port"]);
        private int retErr = int.Parse(ConfigurationManager.AppSettings["retErr"]);

        public string[] messageTypes = { "CON", "INF", "DEB", "ERR", "FTL" };
        public enum types { CON, INF, DEB, ERR, FTL };

        public int RunClient(string ip, int type, string id, string name, string mesContents)
        {
            return ConnectToLogger(ip, type, id, name, mesContents, serverPort); 
        }

        public int RunRateLimiterTest(string ip, int type, string id, string name, string mesContents) 
        {
            while (true)
            {
                if (ConnectToLogger(ip, type, id, name, mesContents, serverPort) == retErr)
                {
                    return retErr;
                }
            }
        }

        private int ConnectToLogger(string ip, int type, string id, string name, string mesContents, int port)
        {
            try
            {
                TcpClient client = new TcpClient(ip, port);

                NetworkStream stream = client.GetStream();

                byte[] message = CreateMessage(type, id, name, mesContents);

                stream.Write(message, 0, message.Length);

                message = CreateMessage((int)types.INF, id, name, "disconnected");

                stream.Write(message, 0, message.Length); 

                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException)
            {
                return retErr;
            }
            catch (SocketException)
            {
                return retErr;
            }

            return 0;
        }

        private byte[] CreateMessage(int type, string id, string name, string mesContents) 
        {
            string contents = string.Empty;
            object messageObj;

            switch(type)
            {
                case (int)types.CON:
                    messageObj = new { ID = messageTypes[(int)types.INF], DeviceName = name, GUID = id };
                    contents = JsonSerializer.Serialize(messageObj); 
                    break;

                case (int)types.INF:
                    messageObj = new { ID = messageTypes[type], DeviceName = name, GUID = id, Contents = mesContents };
                    contents = JsonSerializer.Serialize(messageObj);
                    break;

                case (int)types.DEB:
                    messageObj = new { ID = messageTypes[type], DeviceName = name, GUID = id, Contents = mesContents };
                    contents = JsonSerializer.Serialize(messageObj);
                    break;

                case (int)types.ERR:
                    messageObj = new { ID = messageTypes[type], DeviceName = name, GUID = id, Contents = mesContents };
                    contents = JsonSerializer.Serialize(messageObj);
                    break;

                case (int)types.FTL:
                    messageObj = new { ID = messageTypes[type], DeviceName = name, GUID = id, Contents = mesContents };
                    contents = JsonSerializer.Serialize(messageObj);
                    break;

                default:
                    break;
            }

            byte[] message = Encoding.ASCII.GetBytes(contents);

            return message;
        }

    }
}
