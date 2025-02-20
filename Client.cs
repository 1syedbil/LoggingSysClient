using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace NetworkingA3Client
{
    internal class Client
    {

        public void RunClient()
        {
            ConnectToLogger("127.0.0.1", 13000);
        }

        private void ConnectToLogger(string ip, int port)
        {
            try
            {
                TcpClient client = new TcpClient(ip, port);

                NetworkStream stream = client.GetStream();

                stream.Write(CreateMessage(), 0, 5);

                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private byte[] CreateMessage()
        {
            string contents = "Hello";
            byte[] message = Encoding.ASCII.GetBytes(contents);

            return message;
        }

    }
}
