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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GobangClient
{
    /// <summary>
    /// Interaction logic for SearchForGameWindow.xaml
    /// </summary>
    public partial class SearchForGameWindow : Window
    {
        public SearchForGameWindow()
        {
            InitializeComponent();
        }

        private void lstIdleUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void lstPlayingUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Communication.Send(JsonPackageKeys.RequestForUserList, "");
            
        }
    }
}
