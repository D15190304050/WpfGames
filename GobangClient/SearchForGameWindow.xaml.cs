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
        //private BackgroundWorker matchListener;
        private Thread matchListener;
        private Timer requestForUserListsTimer;
        private JToken finalMatchInfo;
        private bool stopListening;

        public SearchForGameWindow(string localAccount)
        {
            this.localAccount = localAccount;
            stopListening = false;
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

            //matchListener = new BackgroundWorker
            //{
            //    WorkerReportsProgress = true,
            //    WorkerSupportsCancellation = true,
            //};

            matchListener = new Thread(ListenMatch);
            matchListener.Start();

            Thread startGame = new Thread(() =>
            {
                for (;;)
                {
                    if (matchListener.ThreadState == ThreadState.Stopped)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            new MainScene(localAccount, finalMatchInfo).Show();
                            this.Close();
                        });
                        return;
                    }

                    Thread.Sleep(1000); 
                }
            });
            startGame.Start();

            //matchListener.DoWork += ListenMatch;
            //matchListener.RunWorkerCompleted += EndStartMatch;
            //matchListener.RunWorkerAsync();
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

        private void ListenMatch()
        {
            for (;;)
            {
                //BackgroundWorker worker = sender as BackgroundWorker;
                //if (worker.CancellationPending)
                //{
                //    e.Cancel = true;
                //    return;
                //}

                if (stopListening)
                    return;

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
                            ResponseMatchRequest(responseMessage[JsonPackageKeys.Body]);
                            break;
                        case JsonPackageKeys.AcceptMatch:
                            MessageBox.Show("对方接受了您的比赛请求");
                            BeginStartMatch(responseMessage[JsonPackageKeys.Body]);
                            break;
                        case JsonPackageKeys.UserList:
                            RefreshUserLists(responseMessage[JsonPackageKeys.Body]);
                            break;
                        case JsonPackageKeys.RejectMatch:
                            MessageBox.Show("对方拒绝了您的比赛请求");
                            break;
                        default:
                            MessageBox.Show(JsonPackageKeys.UnknownError + " in SearchForGameWindow\n" + "Raised by " + responseMessage[JsonPackageKeys.Body][JsonPackageKeys.Receiver] + "\n" + responseMessage);
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

        private void ResponseMatchRequest(JToken matchInfo)
        {
            string initiatorAccount = matchInfo[JsonPackageKeys.InitiatorAccount].ToString();

            // If accept the match request, send the acceptance response, close this window and show the match window.
            // Else, send the rejection response.
            if (AcceptMatch(initiatorAccount))
            {
                Communication.Send(JsonPackageKeys.AcceptMatch, matchInfo);
                BeginStartMatch(JObject.FromObject(matchInfo));
            }
            else
                Communication.Send(JsonPackageKeys.RejectMatch, matchInfo);
        }

        // Start game.
        private void BeginStartMatch(JToken matchInfo)
        {
            finalMatchInfo = matchInfo;
            requestForUserListsTimer.Dispose();
            matchListener.Abort();
            this.Dispatcher.Invoke(() =>
            {
                //matchListener.CancelAsync();
                stopListening = true;
            });
        }

        private void EndStartMatch(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                new MainScene(localAccount, finalMatchInfo).Show();
                this.Close();
            });
        }
    }
}
