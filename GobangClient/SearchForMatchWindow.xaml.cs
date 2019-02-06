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
    public partial class SearchForMatchWindow : Window
    {
        private string localAccount;

        public JToken FinalMatchInfo { get; private set; }

        public MainScene MainScene { get; set; }

        public SearchForMatchWindow(string localAccount)
        {
            this.localAccount = localAccount;
            InitializeComponent();
        }

        private void lstIdleUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lstIdleUsers.SelectedItem is string opponentAccount)
            {
                RequestForMatch(opponentAccount);
                MessageBox.Show("请求已发送");
            }
        }

        // Start to request user lists once the window is loaded.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            new Timer(RequestUserLists, null, 0, 2000);
        }

        // Refresh the idle user list and the playing user list every 2 seconds.
        // Use a method to encapsulate this function to enhance readability.
        private void RequestUserLists(object state)
        {
            Communication.Send(JsonPackageKeys.RequestForUserList, "");
        }

        private void RequestForMatch(string opponentAccount)
        {
            object matchRequest = new
            {
                InitiatorAccount = localAccount,
                OpponentAccount = opponentAccount
            };
            Communication.Send(JsonPackageKeys.RequestForMatch, matchRequest);
        }

        public void RefreshUserLists(JToken userList)
        {
            // Clear all existing user information.
            this.Dispatcher.Invoke(() =>
            {
                lstIdleUsers.Items.Clear();
                lstPlayingUsers.Items.Clear();
            });

            // Get the user lists and their length.
            int idleUserCount = int.Parse(userList[JsonPackageKeys.IdleUserCount].ToString());
            int playingUserCount = int.Parse(userList[JsonPackageKeys.PlayingUserCount].ToString());
            JToken idleUsers = userList[JsonPackageKeys.IdleUsers];
            JToken playingUsers = userList[JsonPackageKeys.PlayingUsers];

            // Add idle users' information to the window.
            for (int i = 0; i < idleUserCount; i++)
            {
                string account = idleUsers[i][JsonPackageKeys.Account].ToString();

                if (string.IsNullOrEmpty(account))
                    continue;

                if (account != localAccount)
                    this.Dispatcher.Invoke(() => lstIdleUsers.Items.Add(account));
            }

            // Add playing users' information to the window.
            for (int i = 0; i < playingUserCount; i++)
            {
                string account = playingUsers[i][JsonPackageKeys.Account].ToString();
                if (string.IsNullOrEmpty(account))
                    continue;
                this.Dispatcher.Invoke(() => lstPlayingUsers.Items.Add(account));
            }

            // Call Refresh() so that the UI will be updated.
            this.Dispatcher.Invoke(() =>
            {
                lstIdleUsers.Items.Refresh();
                lstPlayingUsers.Items.Refresh();
            });
        }

        private bool AcceptMatch(string initiatorAccount)
        {
            MessageBoxResult result = MessageBox.Show("用户 " + initiatorAccount + " 向您发出了比赛请求，是否接受？", "比赛请求", MessageBoxButton.YesNo);
            return result == MessageBoxResult.Yes;
        }

        public void ResponseMatchRequest(JToken matchInfo)
        {
            string initiatorAccount = matchInfo[JsonPackageKeys.InitiatorAccount].ToString();

            // If accept the match request, send the acceptance response, close this window and show the match window.
            // Else, send the rejection response.
            if (AcceptMatch(initiatorAccount))
            {
                Communication.Send(JsonPackageKeys.AcceptMatch, matchInfo);
                StartMatch(JObject.FromObject(matchInfo));
            }
            else
                Communication.Send(JsonPackageKeys.RejectMatch, matchInfo);
        }

        // Start game.
        public void StartMatch(JToken matchInfo)
        {
            FinalMatchInfo = matchInfo;
            //matchListener.Abort();
            this.Dispatcher.Invoke(() =>
            {
                //matchListener.CancelAsync();
                //stopListening = true;
                this.Hide();
                MainScene.MatchInfo = FinalMatchInfo;
                MainScene.Show();
            });
        }
    }
}
