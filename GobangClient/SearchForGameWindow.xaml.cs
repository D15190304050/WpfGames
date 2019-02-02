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
        private BackgroundWorker matchListener;
        private Timer requestForUserListsTimer;

        public SearchForGameWindow(string localAccount)
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            requestForUserListsTimer = new Timer(RequestUserLists, null, 0, 2000);

            matchListener = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true,
            };

            matchListener.DoWork += ListenMatch;
            matchListener.RunWorkerAsync();
        }

        // Refresh the idle user list and the playing user list every 2 seconds.
        // Use a method to encapsulate this function to enhance readability.
        private void RequestUserLists(object state)
        {
            Communication.Send(JsonPackageKeys.RequestForUserList, "");
        }

        // Terminate the matchListener.
        private void Window_Closed(object sender, EventArgs e)
        {
            matchListener.CancelAsync();
            requestForUserListsTimer.Dispose();
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

        private void ListenMatch(object sender, DoWorkEventArgs e)
        {
            for (;;)
            {
                JObject[] responseMessages = Communication.ReceiveMessages();
                for (int i = 0; i < responseMessages.Length; i++)
                {
                    JObject responseMessage = responseMessages[i];
                    switch (responseMessage[JsonPackageKeys.Type].ToString())
                    {
                        case JsonPackageKeys.OpponentNotAvailable:
                            MessageBox.Show(JsonPackageKeys.OpponentNotAvailable);
                            break;
                        case JsonPackageKeys.RequestForMatch:
                            ResponseMatchRequest(responseMessage[JsonPackageKeys.Body][JsonPackageKeys.InitiatorAccount]
                                .ToString());
                            break;
                        case JsonPackageKeys.AcceptMatch:
                            MessageBox.Show("对方接受了您的比赛请求");
                            StartMatch();
                            break;
                        case JsonPackageKeys.UserList:
                            RefreshUserLists(responseMessage[JsonPackageKeys.Body]);
                            break;
                        case JsonPackageKeys.RejectMatch:
                            MessageBox.Show("对方拒绝了您的比赛请求");
                            break;
                        default:
                            MessageBox.Show(JsonPackageKeys.UnknownError + "\n" + responseMessage);
                            break;
                    }
                }
            }
        }

        private void RefreshUserLists(JToken userList)
        {
            // Clear all existing user information.
            this.Dispatcher.Invoke(() => lstIdleUsers.Items.Clear());
            this.Dispatcher.Invoke(() => lstPlayingUsers.Items.Clear());

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
        }

        private bool AcceptMatch(string initiatorAccount)
        {
            MessageBoxResult result = MessageBox.Show("用户 " + initiatorAccount + " 向您发出了比赛请求，是否接受？", "比赛请求", MessageBoxButton.YesNo);
            return result == MessageBoxResult.Yes;
        }

        private void ResponseMatchRequest(string initiatorAccount)
        {
            object matchInfo = new
            {
                InitiatorAccount = initiatorAccount,
                OpponentAccount = localAccount
            };

            // If accept the match request, send the acceptance response, close this window and show the match window.
            // Else, send the rejection response.
            if (AcceptMatch(initiatorAccount))
            {
                Communication.Send(JsonPackageKeys.AcceptMatch, matchInfo);
                StartMatch();
            }
            else
                Communication.Send(JsonPackageKeys.RejectMatch, matchInfo);
        }

        // Start game.
        private void StartMatch()
        {
            this.Dispatcher.Invoke(() => new MainScene().Show());
            this.Dispatcher.Invoke(this.Close);
        }
    }
}
