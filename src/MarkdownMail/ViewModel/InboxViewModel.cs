using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using MailKit;
using MailKit.Net.Imap;

namespace Walterlv.MarkdownMail
{
    public sealed class InboxViewModel : MailFolderViewModel
    {
        public InboxViewModel() : base(new ImapClient(), client => client.Inbox)
        {
#if DEBUG
            //if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                Mails.Add(new MailViewModel("Git of markdown-mail project")
                {
                    FormattedBrief = "walterlv started a discussion to lindexi's Merge Request.",
                });
                Mails.Add(new MailViewModel("markdown-mail 项目版本管理")
                {
                    FormattedBrief = "walterlv 在 lindexi 的 Merge Request 中添加了一个讨论。",
                });
                Mails.Add(new MailViewModel("markdown-mail 項目版本管理")
                {
                    FormattedBrief = "walterlv 在 lindexi 的 Merge Request 中添加了一個討論。",
                });
                Mails.Add(new MailViewModel("マークダウンメールプロジェクトのバージョン管理")
                {
                    FormattedBrief = "walterlvさんがlindexiのMerge Requestのディスカッションに追加しました。",
                });
            }
#endif
        }

        public async Task Connect(string userName, string password)
        {
            var client = Client;

            await client.ConnectAsync("imap.cvte.com", 993, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(userName, password);

            await UpdateFoldersPrivateAsync();

            OnPropertyChanged(nameof(Name));
        }
    }
}
