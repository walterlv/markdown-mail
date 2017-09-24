using System.Collections.ObjectModel;

namespace Walterlv.MarkdownMail
{
    public class VariableDefinitionRuleSetViewModel : NotificationObject
    {
        public string MailBoxId { get; set; }

        public ObservableCollection<VariableDefinitionRuleViewModel> Rules { get; } =
            new ObservableCollection<VariableDefinitionRuleViewModel>();
    }
}
