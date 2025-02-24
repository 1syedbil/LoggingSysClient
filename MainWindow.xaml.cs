using System;
using System.Collections.Generic;
using System.Configuration;
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
            UniqueId = Guid.NewGuid().ToString();  
            DeviceName = Environment.MachineName;
        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            Ip = ipInput.Text;  
            if (client.RunClient(Ip, (int)Client.types.CON, UniqueId, DeviceName) == retErr) 
            {
                MessageBoxResult result = MessageBox.Show("Could not connect to server. The client will close upon exit from this window.", "Connection Error");
                if (result == MessageBoxResult.OK || result == MessageBoxResult.None)
                {
                    Close();
                }
            }
        }
    }
}
