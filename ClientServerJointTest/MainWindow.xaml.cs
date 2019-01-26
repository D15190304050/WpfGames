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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using GobangClient;

namespace ClientServerJointTest
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void cmdNewWindow_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button) e.OriginalSource;

            Type type = this.GetType();
            Assembly assembly = type.Assembly;
            Window window = (Window) assembly.CreateInstance(type.Namespace + "." + button.Tag);

            window.Show();
        }

        private void cmdStartGobangServer_Click(object sender, RoutedEventArgs e)
        {
            GobangServerWindow serverWindow = new GobangServerWindow();
            serverWindow.Show();
        }

        private void cmdStartGobangClient_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }
}
