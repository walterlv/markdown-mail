using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Walterlv.MarkdownMail
{
    public sealed partial class BriefRuleEditingPage : Page
    {
        public BriefRuleEditingPage()
        {
            InitializeComponent();

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            _timer.Tick += OnTick;

            Loaded += OnLoaded;
        }

        private readonly DispatcherTimer _timer;

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            var ruleSet =
                await JsonSettings.ReadAsync<VariableDefinitionRuleSetViewModel>(
                    "VariableRuleSet.json") ?? new VariableDefinitionRuleSetViewModel();
            DataContext = ruleSet;
            if (!ruleSet.Rules.Any())
            {
                ruleSet.Rules.Add(new VariableDefinitionRuleViewModel());
            }
            RuleListView.SelectedIndex = 0;
        }

        private void AddVariableButton_Click(object sender, RoutedEventArgs e)
        {
            if (OneRulePanel.DataContext is VariableDefinitionRuleViewModel rule)
            {
                rule.VariableDefinitions.Add(new VariableDefinitionViewModel());
            }
        }

        private async void SaveVariableButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is VariableDefinitionRuleSetViewModel rule)
            {
                await JsonSettings.StoreAsync("VariableRuleSet.json", rule);
            }
        }

        private void RemoveVariableButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.DataContext is VariableDefinitionViewModel variable)
            {
                if (OneRulePanel.DataContext is VariableDefinitionRuleViewModel rule)
                {
                    rule.VariableDefinitions.Remove(variable);
                }
            }
        }

        private void PreviewMailSubject_TextChanged(object sender, TextChangedEventArgs e)
        {
            StartPreview();
        }

        private void PreviewMailBody_TextChanged(object sender, TextChangedEventArgs e)
        {
            StartPreview();
        }

        private void CustomFormat_TextChanged(object sender, TextChangedEventArgs e)
        {
            StartPreview();
        }

        private void Variable_TextChanged(object sender, TextChangedEventArgs e)
        {
            StartPreview();
        }

        private void Regex_TextChanged(object sender, TextChangedEventArgs e)
        {
            StartPreview();
        }

        private void StartPreview()
        {
            _timer.Stop();
            _timer.Start();
        }

        private void OnTick(object sender, object e)
        {
            _timer.Stop();
            Preview();
        }

        private void Preview()
        {
            if (OneRulePanel.DataContext is VariableDefinitionRuleViewModel rule)
            {
                var originSubject = PreviewMailSubjectBox.Text;
                var originBody = PreviewMailBodyBox.Text;
                var format = FormatBox.Text;
                var text = new StringBuilder(format);
                foreach (var var in rule.VariableDefinitions
                    .Where(x => !string.IsNullOrWhiteSpace(x.Name)
                                && !string.IsNullOrWhiteSpace(x.Regex)))
                {
                    Match match;
                    try
                    {
                        switch (var.Target)
                        {
                            case VariableMatchingTarget.Subject:
                                match = Regex.Match(originSubject, var.Regex);
                                break;
                            case VariableMatchingTarget.Body:
                                match = Regex.Match(originBody, var.Regex);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    catch (ArgumentException)
                    {
                        continue;
                    }
                    if (match.Success)
                    {
                        var value = match.Groups[var.GroupIndex].Value;
                        text.Replace($"{{{var.Name}}}", value);
                    }
                }
                PreviewMailEffectBox.Text = text.ToString();
            }
        }
    }
}
