using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Imap;
using MimeKit.Text;

namespace Walterlv.MarkdownMail
{
    public class MailFolderViewModel : MailViewModel
    {
        public MailFolderViewModel(ImapClient client, IMailFolder folder)
        {
            Client = client;
            _getFolder = x => folder;
        }

        internal MailFolderViewModel(ImapClient client, Func<ImapClient, IMailFolder> getFolder)
        {
            Client = client;
            _getFolder = getFolder;
        }

        private readonly Func<ImapClient, IMailFolder> _getFolder;
        private bool _isReceiving;

        protected IMailFolder MailFolder => _getFolder(Client);

        protected ImapClient Client { get; }

        public string Name => MailFolder.FullName;

        public int TotalMessageCount { get; protected set; }

        public int RecentMessageCount { get; protected set; }

        public ObservableCollection<MailDataViewModel> Mails { get; } = new ObservableCollection<MailDataViewModel>();

        public ObservableCollection<MailFolderViewModel> SubFolders { get; } = new ObservableCollection<MailFolderViewModel>();

        public bool IsReceiving
        {
            get => _isReceiving;
            private set
            {
                if (_isReceiving == value) return;
                _isReceiving = value;
                OnPropertyChanged();
            }
        }

        protected async Task UpdateFoldersPrivateAsync()
        {
            var client = Client;
            var folder = MailFolder;
            var subFolders = await folder.GetSubfoldersAsync();
            SubFolders.Clear();
            SubFolders.Add(new MailFolderViewModel(client, folder));
            foreach (var sub in subFolders)
            {
                var f = new MailFolderViewModel(client, sub);
                SubFolders.Add(f);
                await f.UpdateFoldersPrivateAsync();
            }
        }


        public async Task ReceiveAsync(int count = int.MaxValue)
        {
            if (IsReceiving || Mails.Any()) return;
            try
            {
                IsReceiving = true;
                Mails.Clear();
                await ReceivePrivateAsync(count);
            }
            finally
            {
                IsReceiving = false;
            }
        }

        private async Task ReceivePrivateAsync(int count = int.MaxValue)
        {
            var folder = MailFolder;
            folder.Open(FolderAccess.ReadOnly);
            try
            {
                TotalMessageCount = folder.Count;
                RecentMessageCount = folder.Recent;

                var receivedCount = 0;
                for (var i = folder.Count - 1; i >= 0; i--)
                {
                    var message = await folder.GetMessageAsync(i);
                    Mails.Add(new MailDataViewModel(message.Subject)
                    {
                        FormattedBrief = message.GetTextBody(TextFormat.Plain),
                    });
                    receivedCount++;
                    if (receivedCount >= count)
                    {
                        break;
                    }
                }
            }
            finally
            {
                folder.Close();
            }
        }
    }
}
