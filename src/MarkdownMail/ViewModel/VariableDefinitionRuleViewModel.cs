using System.Collections.ObjectModel;

namespace Walterlv.MarkdownMail
{
    public class VariableDefinitionRuleViewModel : NotificationObject
    {
        public ObservableCollection<VariableDefinitionViewModel> VariableDefinitions { get; } =
            new ObservableCollection<VariableDefinitionViewModel>();
    }
}
