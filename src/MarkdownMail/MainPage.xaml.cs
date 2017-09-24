using System;
using System.Linq;
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
            await InboxViewModel.Connect(UserNameBox.Text, PasswordBox.Password);
        }

        private async void Folder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var folder = e.AddedItems.FirstOrDefault() as MailFolderViewModel;
            ContentPanel.DataContext = e.AddedItems.FirstOrDefault();
            if (folder != null)
            {
                await folder.ReceiveAsync(10);
            }
        }

        private void AddVariableButton_Click(object sender, RoutedEventArgs e)
        {
            if (VariableItem.DataContext is VariableDefinitionRuleViewModel rule)
            {
                rule.VariableDefinitions.Add(new VariableDefinitionViewModel());
            }
        }
    }
}
