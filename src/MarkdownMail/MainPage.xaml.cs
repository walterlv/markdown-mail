using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            var mailBoxInfo = await JsonSettings.ReadAsync<MailBoxInfo>("Mails.json");
            HostBox.Text = mailBoxInfo?.ImapHost ?? "";
            UserNameBox.Text = mailBoxInfo?.UserName ?? "";
            PasswordBox.Focus(FocusState.Programmatic);
        }

        public InboxViewModel InboxViewModel => (InboxViewModel) DataContext;

        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            await InboxViewModel.ConnectAsync(HostBox.Text, UserNameBox.Text, PasswordBox.Password);
            await JsonSettings.StoreAsync("Mails.json", new MailBoxInfo
            {
                ImapHost = HostBox.Text,
                UserName = UserNameBox.Text,
            });
            Pivot.SelectedIndex = 1;
        }

        private async void Folder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var folder = e.AddedItems.FirstOrDefault() as MailFolderViewModel;
            ContentFrame.DataContext = e.AddedItems.FirstOrDefault();
            if (folder != null)
            {
                await folder.ReceiveAsync(10);
            }
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (Pivot.SelectedIndex)
            {
                case 0:
                    ContentFrame.Content = null;
                    break;
                case 1:
                    ContentFrame.Navigate(typeof(NormalMailBoxPage));
                    break;
                case 2:
                    ContentFrame.Navigate(typeof(BriefRuleEditingPage));
                    break;
            }
        }
    }
}
