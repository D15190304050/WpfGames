using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Threading;

namespace GobangClient
{
    /// <summary>
    /// Interaction logic for SearchForGameWindow.xaml
    /// </summary>
    public partial class SearchForGameWindow : Window
    {
        private string localAccount;

        public SearchForGameWindow(string localAccount)
        {
            this.localAccount = localAccount;
            InitializeComponent();
        }

        private void lstIdleUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lstIdleUsers.SelectedItem is string opponentAccount)
            {
                MessageBox.Show("对手: " + opponentAccount);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true,
            };

            worker.DoWork += RequestUserLists;
            worker.RunWorkerAsync();
        }

        private void RequestUserLists(object sender, DoWorkEventArgs e)
        {
            for (;;)
            {
                // Clear all existing user information.
                this.Dispatcher.Invoke(() => lstIdleUsers.Items.Clear());
                this.Dispatcher.Invoke(() => lstPlayingUsers.Items.Clear());

                Communication.Send(JsonPackageKeys.RequestForUserList, "");
                JToken userList = Communication.Receive()[JsonPackageKeys.Body];

                int idleUserCount = int.Parse(userList[JsonPackageKeys.IdleUserCount].ToString());
                int playingUserCount = int.Parse(userList[JsonPackageKeys.PlayingUserCount].ToString());
                JToken idleUsers = userList[JsonPackageKeys.IdleUsers];
                JToken playingUsers = userList[JsonPackageKeys.PlayingUsers];

                for (int i = 0; i < idleUserCount; i++)
                {
                    string account = idleUsers[i][JsonPackageKeys.Account].ToString();

                    if (string.IsNullOrEmpty(account))
                        continue;

                    if (account != localAccount)
                        this.Dispatcher.Invoke(() => lstIdleUsers.Items.Add(account));
                }

                for (int i = 0; i < playingUserCount; i++)
                {
                    string account = playingUsers[i].ToString();
                    this.Dispatcher.Invoke(() => lstIdleUsers.Items.Add(account));
                }

                this.Dispatcher.Invoke(() => lstIdleUsers.Items.Refresh());
                this.Dispatcher.Invoke(() => lstPlayingUsers.Items.Refresh());

                // Sleep for 2000 millis, which makes the request not so frequent.
                Thread.Sleep(2000);
            }
        }
    }
}
