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
            var file = await ApplicationData.Current.LocalFolder.GetFileAsync(
                "VariableRuleSet.json");
            var json = JsonSerializer.Create();
            using (TextReader reader = new StreamReader(await file.OpenStreamForReadAsync()))
            {
                var vm = json.Deserialize<VariableDefinitionRuleViewModel>(
                    new JsonTextReader(reader));
                VariableItem.DataContext = vm;
            }
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

        private async void SaveVariableButton_Click(object sender, RoutedEventArgs e)
        {
            if (VariableItem.DataContext is VariableDefinitionRuleViewModel rule)
            {
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(
                    "VariableRuleSet.json",
                    CreationCollisionOption.ReplaceExisting);
                var json = JsonSerializer.Create();
                using (TextWriter writer = new StreamWriter(await file.OpenStreamForWriteAsync()))
                {
                    json.Serialize(writer, rule);
                }
            }
        }
    }
}
