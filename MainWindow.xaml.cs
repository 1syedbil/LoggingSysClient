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

        private void edgeCaseBtn_Click(object sender, RoutedEventArgs e)
        {
            //send the empty message
            if (client.SendEmptyMsg(Ip) == retErr) 
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

                throw;
            }
        }

        private void rateLimiterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (client.RunRateLimiterTest(Ip, (int)Client.types.DEB, UniqueId, DeviceName, "Hello, this is a rate limiter test.") == retErr)
            {
                MessageBoxResult result = MessageBox.Show("Could not connect to server. The client will close upon exit from this window.", "Connection Error");
                if (result == MessageBoxResult.OK || result == MessageBoxResult.None)
                {
                    Close();
                }
            }
        }

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

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            testePageLabel.Visibility = Visibility.Visible;
            debugBtn.Visibility = Visibility.Visible;
            edgeCaseBtn.Visibility = Visibility.Visible;
            errorBtn.Visibility = Visibility.Visible;
            fatalBtn.Visibility = Visibility.Visible;
            rateLimiterBtn.Visibility = Visibility.Visible;

            msgInputLabel.Visibility = Visibility.Collapsed;
            msgInput.Visibility = Visibility.Collapsed;
            sendMsgBtn.Visibility = Visibility.Collapsed;
            backBtn.Visibility = Visibility.Collapsed;
        }
    }
}
