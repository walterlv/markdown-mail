using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Walterlv.MarkdownMail
{
    public class VariableDefinitionRuleViewModel : NotificationObject
    {
        public string From { get; set; }

        public ObservableCollection<VariableDefinitionViewModel> VariableDefinitions { get; } =
            new ObservableCollection<VariableDefinitionViewModel>();

        public string CustomFormat { get; set; }

        public string PreviewSubject { get; set; }

        public string PreviewBody { get; set; }

        public string Format(string subject, string body)
        {
            var text = new StringBuilder(CustomFormat);
            foreach (var var in VariableDefinitions
                .Where(x => !string.IsNullOrWhiteSpace(x.Name)
                            && !string.IsNullOrWhiteSpace(x.Regex)))
            {
                Match match;
                try
                {
                    switch (var.Target)
                    {
                        case VariableMatchingTarget.Subject:
                            match = Regex.Match(subject, var.Regex);
                            break;
                        case VariableMatchingTarget.Body:
                            match = Regex.Match(body, var.Regex);
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

            return text.ToString();
        }
    }
}
