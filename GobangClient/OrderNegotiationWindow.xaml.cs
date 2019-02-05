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
using Newtonsoft.Json.Linq;

namespace GobangClient
{
    /// <summary>
    /// Interaction logic for OrderNegotiationWindow.xaml
    /// </summary>
    public partial class OrderNegotiationWindow : Window
    {
        private string localAccount;
        private string opponentAccount;
        private bool confirmed;

        public OrderNegotiationWindow(string localAccount, JToken matchInfo)
        {
            confirmed = false;
            this.localAccount = localAccount;
            string initiatorAccount = matchInfo[JsonPackageKeys.InitiatorAccount].ToString();

            // This opponent here means the opponent of the initiator, instead of the opponent of the current user.
            string opponentAccount = matchInfo[JsonPackageKeys.OpponentAccount].ToString();

            // This opponent here means the opponent of the current user.
            // Note the different meaning of "opponent" in the context.
            this.opponentAccount = localAccount == initiatorAccount ? opponentAccount : initiatorAccount;

            InitializeComponent();
        }

        // This window cannot be closed by clicking the "x" button on the top-right corner of this window.
        // Only click the "Confirm" button can close this window.
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = !confirmed;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            rbLocalAccount.Content = localAccount;

            // This opponent here means the opponent of the current user.
            // Note the different meaning of "opponent" in the context.
            rbOpponentAccount.Content = opponentAccount;
        }

        private void cmdConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (rbLocalAccount.IsChecked == true)
            {
                object orderInfo = new
                {
                    Sender = localAccount,
                    Receiver = opponentAccount,
                    BlackChessPieceUser = localAccount,
                    WhiteChessPieceUser = opponentAccount
                };
                Communication.Send(JsonPackageKeys.Order, orderInfo);
            }
            else
            {
                object orderInfo = new
                {
                    Sender = localAccount,
                    Receiver = opponentAccount,
                    BlackChessPieceUser = opponentAccount,
                    WhiteChessPieceUser = localAccount
                };
                Communication.Send(JsonPackageKeys.Order, orderInfo);
            }

            // Close this window after clicking the confirm button.
            confirmed = true;
            this.Close();
        }
    }
}
