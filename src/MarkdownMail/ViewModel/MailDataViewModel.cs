namespace Walterlv.MarkdownMail
{
    public class MailDataViewModel : MailViewModel
    {
        public MailDataViewModel(string subject)
        {
            Subject = subject;
        }

        private string _body;

        public string Subject { get; }

        public string From { get; set; }

        public string Body
        {
            get => _body;
            internal set
            {
                _body = value;
                OnPropertyChanged();
            }
        }
    }
}
