using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Walterlv.MarkdownMail
{
    public sealed partial class BriefRuleEditingPage : Page
    {
        public BriefRuleEditingPage()
        {
            this.InitializeComponent();
        }

        private void AddVariableButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is VariableDefinitionRuleViewModel rule)
            {
                rule.VariableDefinitions.Add(new VariableDefinitionViewModel());
            }
        }

        private async void SaveVariableButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is VariableDefinitionRuleViewModel rule)
            {
                await JsonSettings.StoreAsync("VariableRuleSet.json", rule);
            }
        }

        private void RemoveVariableButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.DataContext is VariableDefinitionViewModel variable)
            {
                if (DataContext is VariableDefinitionRuleViewModel rule)
                {
                    rule.VariableDefinitions.Remove(variable);
                }
            }
        }
    }
}
