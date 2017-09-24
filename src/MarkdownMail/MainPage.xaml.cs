using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MailKit;
using MailKit.Net.Imap;

namespace Walterlv.MarkdownMail
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void ReceiveButton_Click(object sender, RoutedEventArgs e)
        {
            using (var client = new ImapClient())
            {
                client.Connect("imap.cvte.com", 993, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(UserNameBox.Text, PasswordBox.Password);

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                TestTextBlock.Text += $"Total messages: {inbox.Count}{Environment.NewLine}";
                TestTextBlock.Text += $"Recent messages: {inbox.Recent}{Environment.NewLine}";

                for (var i = 0; i < inbox.Count; i++)
                {
                    await Task.Delay(100);
                    var message = inbox.GetMessage(i);
                    TestTextBlock.Text += $"Subject: {message.Subject}{Environment.NewLine}";
                }
            }
        }
    }
}
