using System;
using System.IO;
using System.Linq;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;

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

            VariableItem.DataContext =
                await JsonSettings.ReadAsync<VariableDefinitionRuleViewModel>(
                    "VariableRuleSet.json") ?? new VariableDefinitionRuleViewModel();
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

        private async void SaveVariableButton_Click(object sender, RoutedEventArgs e)
        {
            if (VariableItem.DataContext is VariableDefinitionRuleViewModel rule)
            {
                await JsonSettings.StoreAsync("VariableRuleSet.json", rule);
            }
        }
    }
}
