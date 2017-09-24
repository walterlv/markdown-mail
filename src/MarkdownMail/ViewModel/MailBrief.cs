namespace Walterlv.MarkdownMail
{
    public class MailBrief : NotificationObject
    {
        public MailBrief(string subject)
        {
            Subject = subject;
        }

        private string _formattedBrief;

        public string Subject { get; }

        public string FormattedBrief
        {
            get => _formattedBrief;
            internal set
            {
                _formattedBrief = value;
                OnPropertyChanged();
            }
        }
    }
}
