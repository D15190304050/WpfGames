using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using GobangServer;

namespace ClientServerJointTest
{
    /// <summary>
    /// Interaction logic for GobangServerWindow.xaml
    /// </summary>
    public partial class GobangServerWindow : Window
    {
        private bool started;

        public GobangServerWindow()
        {
            started = false;
            InitializeComponent();
        }

        private void cmdStartServer_Click(object sender, RoutedEventArgs e)
        {
            if (started)
                return;
            started = true;

            lstServerOutput.Items.Add("服务器已启动");
            GameServer.Report += s =>
            {
                this.Dispatcher.Invoke(() => lstServerOutput.Items.Add(s));
            };
            GameServer.Start();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            GameServer.Close();
        }
    }
}
