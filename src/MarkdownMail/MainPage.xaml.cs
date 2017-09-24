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

        public InboxViewModel InboxViewModel => (InboxViewModel) DataContext;

        private async void ReceiveButton_Click(object sender, RoutedEventArgs e)
        {
            await InboxViewModel.ReceiveAsync(UserNameBox.Text, PasswordBox.Password);
        }
    }
}
