namespace Walterlv.MarkdownMail
{
    public class VariableDefinitionViewModel : NotificationObject
    {
        private string _name;
        private string _regex;
        private bool _isSubjectMatching;
        private bool _isBodyMatching = true;
        private VariableMatchingTarget _target = VariableMatchingTarget.Body;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Regex
        {
            get => _regex;
            set
            {
                _regex = value;
                OnPropertyChanged();
            }
        }

        public bool IsSubjectMatching
        {
            get => _isSubjectMatching;
            set
            {
                if (value == _isSubjectMatching)
                {
                    return;
                }
                _isSubjectMatching = value;
                OnPropertyChanged();
                if (value)
                {
                    Target = VariableMatchingTarget.Subject;
                    IsBodyMatching = false;
                }
            }
        }

        public bool IsBodyMatching
        {
            get => _isBodyMatching;
            set
            {
                if (value == _isBodyMatching)
                {
                    return;
                }
                _isBodyMatching = value;
                OnPropertyChanged();
                if (value)
                {
                    Target = VariableMatchingTarget.Body;
                    IsSubjectMatching = false;
                }
            }
        }

        public VariableMatchingTarget Target
        {
            get => _target;
            set
            {
                if (_target == value)
                {
                    return;
                }
                _target = value;
                OnPropertyChanged();
            }
        }
    }
}
