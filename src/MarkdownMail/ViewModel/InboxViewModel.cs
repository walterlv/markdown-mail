using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Imap;

namespace Walterlv.MarkdownMail
{
    public class InboxViewModel : NotificationObject
    {
        public InboxViewModel()
        {
#if DEBUG
            //if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                Mails.Add(new MailBrief("Git of markdown-mail project")
                {
                    FormattedBrief = "walterlv started a discussion to lindexi's Merge Request.",
                });
                Mails.Add(new MailBrief("markdown-mail 项目版本管理")
                {
                    FormattedBrief = "walterlv 在 lindexi's 的 Merge Request 中添加了一个讨论。",
                });
                Mails.Add(new MailBrief("markdown-mail 項目版本管理")
                {
                    FormattedBrief = "walterlv 在 lindexi's 的 Merge Request 中添加了一個討論。",
                });
                Mails.Add(new MailBrief("マークダウンメールプロジェクトのバージョン管理")
                {
                    FormattedBrief = "walterlvさんがlindexiのMerge Requestのディスカッションに追加しました。",
                });
            }
#endif
        }

        private readonly ImapClient _client = new ImapClient();
        private bool _isReceiving;

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

        public int TotalMessageCount { get; private set; }

        public int RecentMessageCount { get; private set; }

        public ObservableCollection<MailBrief> Mails { get; } = new ObservableCollection<MailBrief>();

        public ObservableCollection<FolderBrief> SubFolders { get; } = new ObservableCollection<FolderBrief>();

        public async Task ReceiveAsync(string userName, string password)
        {
            Mails.Clear();
            if (IsReceiving) return;
            try
            {
                IsReceiving = true;
                await ReceivePrivateAsync(userName, password);
            }
            finally
            {
                IsReceiving = false;
            }
        }

        private async Task ReceivePrivateAsync(string userName, string password)
        {
            var client = _client;

            await client.ConnectAsync("imap.cvte.com", 993, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(userName, password);

            var inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadOnly);

            TotalMessageCount = inbox.Count;
            RecentMessageCount = inbox.Recent;

            for (var i = inbox.Count - 1; i >= 0; i--)
            {
                await Task.Delay(200);
                var message = inbox.GetMessage(i);
                Mails.Add(new MailBrief(message.Subject)
                {
                });
            }
        }
    }
}
