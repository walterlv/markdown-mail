using System.Collections.ObjectModel;

namespace Walterlv.MarkdownMail
{
    public class VariableDefinitionRuleViewModel : NotificationObject
    {
        public ObservableCollection<VariableDefinitionViewModel> VariableDefinitions { get; } =
            new ObservableCollection<VariableDefinitionViewModel>();
    }

    public class VariableDefinitionRuleSetViewModel : NotificationObject
    {
        public ObservableCollection<VariableDefinitionRuleViewModel> Rules { get; } =
            new ObservableCollection<VariableDefinitionRuleViewModel>();
    }

    public static class VariableDefinitionRuleSetExtensions
    {
        public static VariableDefinitionRule ToModel(this VariableDefinitionRuleViewModel vm)
        {
            var model = new VariableDefinitionRule();
            foreach (var definition in vm.VariableDefinitions)
            {
                model.Variables.Add(definition.ToModel());
            }
            return model;
        }

        public static VariableDefinition ToModel(this VariableDefinitionViewModel vm)
        {
            return new VariableDefinition
            {
                Name = vm.Name,
                Regex = vm.Regex,
                Target = vm.Target,
            };
        }
    }
}
