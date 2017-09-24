using System.Collections.Generic;

namespace Walterlv.MarkdownMail
{
    public class VariableDefinition
    {
        public string Name { get; set; }

        public string Regex { get; set; }

        public VariableMatchingTarget Target { get; set; }
    }

    public class VariableDefinitionRule
    {
        public IList<string> Senders { get; set; }

        public IList<VariableDefinition> Variables { get; set; }
    }

    public class VariableDefinitionRuleSet
    {
        public IList<VariableDefinitionRule> Rules { get; set; }
    }
}
