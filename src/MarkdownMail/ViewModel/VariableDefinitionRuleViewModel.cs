using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Walterlv.MarkdownMail
{
    public class VariableDefinitionRuleViewModel : NotificationObject
    {
        public string Sender { get; set; }

        public ObservableCollection<VariableDefinitionViewModel> VariableDefinitions { get; } =
            new ObservableCollection<VariableDefinitionViewModel>();

        public string CustomFormat { get; set; }

        public string PreviewSubject { get; set; }

        public string PreviewBody { get; set; }
    }
}
