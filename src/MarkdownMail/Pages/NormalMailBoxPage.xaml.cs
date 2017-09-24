using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Walterlv.MarkdownMail
{
    public sealed partial class NormalMailBoxPage : Page
    {
        public NormalMailBoxPage()
        {
            this.InitializeComponent();
            Loaded += OnLoaded;
        }

        private IList<VariableDefinitionRuleViewModel> _rules;

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            _rules = (await JsonSettings.ReadAsync<VariableDefinitionRuleSetViewModel>(
                "VariableRuleSet.json"))?.Rules;
        }

        private void Mail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.FirstOrDefault() is MailDataViewModel mail)
            {
                var rule = _rules.FirstOrDefault(x => x.From == mail.From);
                if (rule != null)
                {
                    var formated = rule.Format(mail.Subject, mail.Body);
                    ShowToastNotification(formated);
                }
            }
        }

        private void ShowToastNotification(string text)
        {
            // 1. create element
            var toastTemplate = ToastTemplateType.ToastImageAndText04;
            var toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
            // 2. provide text
            var toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode($"{text}"));
            // 3. provide image
            // var toastImageAttributes = toastXml.GetElementsByTagName("image");
            // ((XmlElement)toastImageAttributes[0]).SetAttribute("src", $"ms-appx:///assets/图片文件名");
            // ((XmlElement)toastImageAttributes[0]).SetAttribute("alt", "logo");
            // 4. duration
            var toastNode = toastXml.SelectSingleNode("/toast");
            ((XmlElement)toastNode)?.SetAttribute("duration", "short");
            // 5. audio
            var audio = toastXml.CreateElement("audio");
            audio.SetAttribute("src", $"ms-winsoundevent:Notification.Default");
            toastNode?.AppendChild(audio);
            // 6. app launch parameter
            // ((XmlElement)toastNode)?.SetAttribute("launch", "{\"type\":\"toast\",\"param1\":\"12345\",\"param2\":\"67890\"}");
            // 7. send toast
            var toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
