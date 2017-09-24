using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Walterlv.MarkdownMail
{
    public class VariableDefinitionRuleViewModel : NotificationObject
    {
        public ObservableCollection<string> Senders { get; }
            = new ObservableCollection<string>();

        public ObservableCollection<VariableDefinitionViewModel> VariableDefinitions { get; } =
            new ObservableCollection<VariableDefinitionViewModel>();
    }
}
