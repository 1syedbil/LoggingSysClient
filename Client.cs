/*
 * FILE          : Client.cs
 * PROJECT       : Network Application Development Assignment 3
 * PROGRAMMER    : Bilal Syed (8927633)
 * FIRST VERSION : 2025-02-20
 * DESCRIPTION   : This file contains the Client class, which is responsible for handling communication with the server.
 *                 It includes methods for sending messages, connecting to the server, and creating messages of different types.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Configuration;
using System.Data.SqlTypes;
using System.Xml.Linq;
using System.Windows;

namespace NetworkingA3Client
{
    internal class Client
    {

        private int serverPort = int.Parse(ConfigurationManager.AppSettings["port"]);
        private int retErr = int.Parse(ConfigurationManager.AppSettings["retErr"]);

        public string[] messageTypes = { "CON", "INF", "DEB", "ERR", "FTL" };
        public enum types { CON, INF, DEB, ERR, FTL };

        /*
         * METHOD        : SendEmptyMsg()
         * DESCRIPTION   : Sends an empty message to the server at the specified IP address.
         * PARAMETERS    : string ip - The IP address of the server.
         *                 string id - The process ID of the client.
         * RETURNS       : int - Returns 0 on success, or retErr on failure.
         */
        public int SendEmptyMsg(string ip, string id)
        {
            try
            {
                TcpClient client = new TcpClient(ip, serverPort);

                NetworkStream stream = client.GetStream();

                object msgObj = new { ID = "", DeviceName = "", PID = id, Contents = "" }; 

                string contents = JsonSerializer.Serialize(msgObj);

                byte[] message = Encoding.ASCII.GetBytes(contents);

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

        /*
         * METHOD        : RunClient()
         * DESCRIPTION   : Connects to the server and sends a message based on the specified type.
         * PARAMETERS    : string ip - The IP address of the server.
         *                 int type - The type of message to send (CON, INF, DEB, ERR, FTL).
         *                 string id - The process ID of the client.
         *                 string name - The name of the device.
         *                 string mesContents - The contents of the message.
         * RETURNS       : int - Returns 0 on success, or retErr on failure.
         */
        public int RunClient(string ip, int type, string id, string name, string mesContents)
        {
            return ConnectToLogger(ip, type, id, name, mesContents, serverPort); 
        }

        /*
         * METHOD        : RunRateLimiterTest()
         * DESCRIPTION   : Continuously sends messages to the server to test the rate limiter.
         * PARAMETERS    : string ip - The IP address of the server.
         *                 int type - The type of message to send (CON, INF, DEB, ERR, FTL).
         *                 string id - The process ID of the client.
         *                 string name - The name of the device.
         *                 string mesContents - The contents of the message.
         * RETURNS       : int - Returns retErr if the connection fails.
         */
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

        /*
         * METHOD        : ConnectToLogger()
         * DESCRIPTION   : Establishes a connection to the server and sends a message.
         * PARAMETERS    : string ip - The IP address of the server.
         *                 int type - The type of message to send (CON, INF, DEB, ERR, FTL).
         *                 string id - The process ID of the client.
         *                 string name - The name of the device.
         *                 string mesContents - The contents of the message.
         *                 int port - The port number to connect to.
         * RETURNS       : int - Returns 0 on success, or retErr on failure.
         */
        private int ConnectToLogger(string ip, int type, string id, string name, string mesContents, int port)
        {
            try
            {
                TcpClient client = new TcpClient(ip, port);

                NetworkStream stream = client.GetStream();

                byte[] message = CreateMessage(type, id, name, mesContents);

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

        /*
         * METHOD        : CreateMessage()
         * DESCRIPTION   : Creates a message based on the specified type and content. Serializes the message into JSON and then
         *                 stores it in a byte array to be sent over the stream.
         * PARAMETERS    : int type - The type of message to create (CON, INF, DEB, ERR, FTL).
         *                 string id - The process ID of the client.
         *                 string name - The name of the device.
         *                 string mesContents - The contents of the message.
         * RETURNS       : byte[] - The serialized message as a byte array.
         */
        private byte[] CreateMessage(int type, string id, string name, string mesContents) 
        {
            string contents = string.Empty;
            object messageObj;

            switch(type)
            {
                case (int)types.CON:
                    messageObj = new { ID = messageTypes[(int)types.INF], DeviceName = name, PID = id };
                    contents = JsonSerializer.Serialize(messageObj); 
                    break;

                case (int)types.INF:
                    messageObj = new { ID = messageTypes[type], DeviceName = name, PID = id, Contents = mesContents };
                    contents = JsonSerializer.Serialize(messageObj);
                    break;

                case (int)types.DEB:
                    messageObj = new { ID = messageTypes[type], DeviceName = name, PID = id, Contents = mesContents };
                    contents = JsonSerializer.Serialize(messageObj);
                    break;

                case (int)types.ERR:
                    messageObj = new { ID = messageTypes[type], DeviceName = name, PID = id, Contents = mesContents };
                    contents = JsonSerializer.Serialize(messageObj);
                    break;

                case (int)types.FTL:
                    messageObj = new { ID = messageTypes[type], DeviceName = name, PID = id, Contents = mesContents };
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
