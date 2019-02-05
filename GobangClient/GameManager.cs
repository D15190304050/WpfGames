using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace GobangClient
{
    public class GameManager
    {
        private static SearchForMatchWindow searchForMatchWindow;
        private static MainScene mainScene;

        public static void StartGame(string localAccount)
        {
            searchForMatchWindow = new SearchForMatchWindow(localAccount);
            mainScene = new MainScene(localAccount, null);
            searchForMatchWindow.mainScene = mainScene;
            mainScene.searchForMatchWindow = searchForMatchWindow;

            searchForMatchWindow.Show();
            mainScene.Hide();

            BackgroundWorker worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true,
            };
            worker.DoWork += ListenServerMessages;
            worker.RunWorkerAsync();
        }

        private static void ListenServerMessages(object sender, DoWorkEventArgs e)
        {
            for (; ; )
            {
                if (sender is BackgroundWorker worker)
                {
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                }

                JObject[] responseMessages = Communication.ReceiveMessages();
                foreach (JObject responseMessage in responseMessages)
                {
                    switch (responseMessage[JsonPackageKeys.Type].ToString())
                    {
                        case JsonPackageKeys.OpponentNotAvailable:
                            MessageBox.Show(JsonPackageKeys.OpponentNotAvailable);
                            break;
                        case JsonPackageKeys.RequestForMatch:
                            searchForMatchWindow.ResponseMatchRequest(responseMessage[JsonPackageKeys.Body]);
                            break;
                        case JsonPackageKeys.AcceptMatch:
                            MessageBox.Show("对方接受了您的比赛请求");
                            searchForMatchWindow.StartMatch(responseMessage[JsonPackageKeys.Body]);
                            break;
                        case JsonPackageKeys.UserList:
                            searchForMatchWindow.RefreshUserLists(responseMessage[JsonPackageKeys.Body]);
                            break;
                        case JsonPackageKeys.RejectMatch:
                            MessageBox.Show("对方拒绝了您的比赛请求");
                            break;
                        case JsonPackageKeys.Order:
                            mainScene.ResponseOrderNegotiation(responseMessage[JsonPackageKeys.Body]);
                            break;
                        case JsonPackageKeys.AcceptOrder:
                            mainScene.StartMatch(responseMessage[JsonPackageKeys.Body]);
                            break;
                        case JsonPackageKeys.ChessPiecePosition:
                            mainScene.ReceiveChessPiecePositionInfo(responseMessage[JsonPackageKeys.Body]);
                            break;
                        case JsonPackageKeys.Win:
                            mainScene.MatchOver("你输了");
                            break;
                        default:
                            MessageBox.Show(JsonPackageKeys.UnknownError + "\n" + responseMessage);
                            break;
                    }
                }
            }
        }
    }
}
