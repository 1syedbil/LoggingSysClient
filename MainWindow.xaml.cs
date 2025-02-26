/*
 * FILE          : MainWindow.xaml.cs
 * PROJECT       : Network Application Development Assignment 3
 * PROGRAMMER    : Bilal Syed (8927633)
 * FIRST VERSION : 2025-02-20
 * DESCRIPTION   : This file contains the MainWindow class, which is the main UI for the client application.
 *                 It handles user interactions, such as connecting to the server, sending messages, and testing edge cases.
 *                 The class also manages the visibility of UI elements based on the application's state.
 */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetworkingA3Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Client client;
        private string ip;
        private int retErr = int.Parse(ConfigurationManager.AppSettings["retErr"]);
        private string uniqueId;
        private string deviceName;

        public string Ip 
        {
           get { return ip; } 
           set {  ip = value; } 
        }

        public string UniqueId
        {
            get { return uniqueId; }
            set { uniqueId = value; }
        }

        public string DeviceName
        {
            get { return deviceName; }
            set { deviceName = value; }
        }

        /*
         * METHOD        : MainWindow()
         * DESCRIPTION   : Initializes the MainWindow and sets up the initial UI state.
         * PARAMETERS    : None
         * RETURNS       : None
         */
        public MainWindow()
        {
            InitializeComponent();
            client = new Client();
            UniqueId = Process.GetCurrentProcess().Id.ToString();  
            DeviceName = Environment.MachineName;

            startBtn.Visibility = Visibility.Visible;
            ipInput.Visibility = Visibility.Visible;
            ipInputLabel.Visibility = Visibility.Visible;
        }

        /*
         * METHOD        : startBtn_Click()
         * DESCRIPTION   : Handles the click event for the start button. Attempts to connect to the server and updates the UI accordingly.
         * PARAMETERS    : object sender - The object that raised the event.
         *                 RoutedEventArgs e - Event arguments.
         * RETURNS       : None
         */
        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            Ip = ipInput.Text;  
            if (client.RunClient(Ip, (int)Client.types.CON, UniqueId, DeviceName, "") == retErr) 
            {
                MessageBoxResult result = MessageBox.Show("Could not connect to server. The client will close upon exit from this window.", "Connection Error");
                if (result == MessageBoxResult.OK || result == MessageBoxResult.None)
                {
                    Close();
                }
            }
            else
            {
                startBtn.Visibility = Visibility.Collapsed;
                ipInput.Visibility = Visibility.Collapsed;
                ipInputLabel.Visibility = Visibility.Collapsed;

                testePageLabel.Visibility = Visibility.Visible;
                debugBtn.Visibility = Visibility.Visible;
                edgeCaseBtn.Visibility = Visibility.Visible;
                errorBtn.Visibility = Visibility.Visible;
                fatalBtn.Visibility = Visibility.Visible;
                rateLimiterBtn.Visibility = Visibility.Visible; 
            }
        }

        /*
         * METHOD        : debugBtn_Click()
         * DESCRIPTION   : Handles the click event for the debug button. Updates the UI to allow sending debug messages.
         * PARAMETERS    : object sender - The object that raised the event.
         *                 RoutedEventArgs e - Event arguments.
         * RETURNS       : None
         */
        private void debugBtn_Click(object sender, RoutedEventArgs e)
        {
            testePageLabel.Visibility = Visibility.Collapsed;
            debugBtn.Visibility = Visibility.Collapsed;
            edgeCaseBtn.Visibility = Visibility.Collapsed;
            errorBtn.Visibility = Visibility.Collapsed;
            fatalBtn.Visibility = Visibility.Collapsed;
            rateLimiterBtn.Visibility = Visibility.Collapsed; 

            msgInputLabel.Visibility = Visibility.Visible;
            msgInput.Visibility = Visibility.Visible;
            sendMsgBtn.Visibility = Visibility.Visible;
            backBtn.Visibility = Visibility.Visible;
        }

        /*
         * METHOD        : edgeCaseBtn_Click()
         * DESCRIPTION   : Handles the click event for the edge case button. Sends an empty message and a large message to the server.
         * PARAMETERS    : object sender - The object that raised the event.
         *                 RoutedEventArgs e - Event arguments.
         * RETURNS       : None
         */
        private void edgeCaseBtn_Click(object sender, RoutedEventArgs e)
        {
            //send the empty message
            if (client.SendEmptyMsg(Ip, UniqueId) == retErr) 
            {
                MessageBoxResult result = MessageBox.Show("Could not connect to server. The client will close upon exit from this window.", "Connection Error");
                if (result == MessageBoxResult.OK || result == MessageBoxResult.None)
                {
                    Close();
                }
            }

            //send the big message
            string message = string.Empty; 

            for (int i = 0; i < int.Parse(ConfigurationManager.AppSettings["bigVal"]); i++)
            {
                message += "a";
            }

            if (client.RunClient(Ip, (int)Client.types.DEB, UniqueId, DeviceName, message) == retErr)  
            {
                MessageBoxResult result = MessageBox.Show("Could not connect to server. The client will close upon exit from this window.", "Connection Error");
                if (result == MessageBoxResult.OK || result == MessageBoxResult.None)
                {
                    Close();
                }
            }
        }

        /*
         * METHOD        : errorBtn_Click()
         * DESCRIPTION   : Handles the click event for the error button. Intentionally causes an error and sends an error message to the server.
         * PARAMETERS    : object sender - The object that raised the event.
         *                 RoutedEventArgs e - Event arguments.
         * RETURNS       : None
         */
        private void errorBtn_Click(object sender, RoutedEventArgs e)
        {
            //cause an intentional error
            try
            {
                int test = retErr / 0;
            }
            //catch the error to send an ERR type message to the logger
            catch (Exception error)
            {
                if (client.RunClient(Ip, (int)Client.types.ERR, UniqueId, DeviceName, error.Message) == retErr) 
                {
                    MessageBoxResult result = MessageBox.Show("Could not connect to server. The client will close upon exit from this window.", "Connection Error");
                    if (result == MessageBoxResult.OK || result == MessageBoxResult.None)
                    {
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show("Exception was caught and sent to server.", "Caught Exception");
                }
            }
        }

        /*
         * METHOD        : fatalBtn_Click()
         * DESCRIPTION   : Handles the click event for the fatal button. Intentionally causes a fatal error and sends a fatal message to the server.
         * PARAMETERS    : object sender - The object that raised the event.
         *                 RoutedEventArgs e - Event arguments.
         * RETURNS       : None
         */
        private void fatalBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                throw new InvalidOperationException("A fatal error has occurred.");
            }
            //catch the error to send an ERR type message to the logger
            catch (Exception error)
            {
                if (client.RunClient(Ip, (int)Client.types.FTL, UniqueId, DeviceName, error.Message) == retErr)
                {
                    MessageBoxResult result = MessageBox.Show("Could not connect to server. The client will close upon exit from this window.", "Connection Error");
                    if (result == MessageBoxResult.OK || result == MessageBoxResult.None)
                    {
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show("Fatal error was logged. Client will crash after you leave this window.", "Fatal Error");
                }

                if (client.RunClient(Ip, (int)Client.types.INF, UniqueId, DeviceName, "disconnected") == retErr) 
                {
                    MessageBoxResult result = MessageBox.Show("Could not connect to server. The client will close upon exit from this window.", "Connection Error");
                    if (result == MessageBoxResult.OK || result == MessageBoxResult.None)
                    {
                        Close();
                    }
                }
                throw;
            }
        }

        /*
         * METHOD        : rateLimiterBtn_Click()
         * DESCRIPTION   : Handles the click event for the rate limiter button. Tests the server's rate limiting by sending multiple messages.
         * PARAMETERS    : object sender - The object that raised the event.
         *                 RoutedEventArgs e - Event arguments.
         * RETURNS       : None
         */
        private async void rateLimiterBtn_Click(object sender, RoutedEventArgs e)
        {
            int result = await Task.Run(() => client.RunRateLimiterTest(Ip, (int)Client.types.DEB, UniqueId, DeviceName, "Hello, this is a rate limiter test."));

            if (result == retErr)
            {
                MessageBoxResult choice = MessageBox.Show("Could not connect to server. The client will close upon exit from this window.", "Connection Error");
                if (choice == MessageBoxResult.OK || choice == MessageBoxResult.None)
                {
                    Close();
                }
            }
        }

        /*
         * METHOD        : sendMsgBtn_Click()
         * DESCRIPTION   : Handles the click event for the send message button. Sends the entered message to the server.
         * PARAMETERS    : object sender - The object that raised the event.
         *                 RoutedEventArgs e - Event arguments.
         * RETURNS       : None
         */
        private void sendMsgBtn_Click(object sender, RoutedEventArgs e)
        {
            string message = msgInput.Text;

            if (client.RunClient(Ip, (int)Client.types.DEB, UniqueId, DeviceName, message) == retErr) 
            {
                MessageBoxResult result = MessageBox.Show("Could not connect to server. The client will close upon exit from this window.", "Connection Error");
                if (result == MessageBoxResult.OK || result == MessageBoxResult.None)
                {
                    Close();
                }
            }
        }

        /*
         * METHOD        : backBtn_Click()
         * DESCRIPTION   : Handles the click event for the back button. Resets the UI to the main test page.
         * PARAMETERS    : object sender - The object that raised the event.
         *                 RoutedEventArgs e - Event arguments.
         * RETURNS       : None
         */
        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            msgInput.Text = string.Empty;

            msgInputLabel.Visibility = Visibility.Collapsed;
            msgInput.Visibility = Visibility.Collapsed;
            sendMsgBtn.Visibility = Visibility.Collapsed;
            backBtn.Visibility = Visibility.Collapsed;

            testePageLabel.Visibility = Visibility.Visible;
            debugBtn.Visibility = Visibility.Visible;
            edgeCaseBtn.Visibility = Visibility.Visible;
            errorBtn.Visibility = Visibility.Visible;
            fatalBtn.Visibility = Visibility.Visible;
            rateLimiterBtn.Visibility = Visibility.Visible;
        }

        /*
         * METHOD        : Window_Closing()
         * DESCRIPTION   : Handles the window closing event. Sends a disconnect message to the server if the client was connected.
         * PARAMETERS    : object sender - The object that raised the event.
         *                 System.ComponentModel.CancelEventArgs e - Event arguments.
         * RETURNS       : None
         */
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (startBtn.Visibility != Visibility.Visible && ipInput.Visibility != Visibility.Visible && ipInputLabel.Visibility != Visibility.Visible)
            {
                if (client.RunClient(Ip, (int)Client.types.INF, UniqueId, DeviceName, "disconnected") == retErr)
                {
                    MessageBox.Show("Could not connect to server. The client will close upon exit from this window.", "Connection Error");
                }
            }
        }
    }
}
