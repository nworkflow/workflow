using Nsun.NSFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Nsun.Tools.WorkflowDesigner.Utils
{
    public class TemplateHistory:ViewModelBase
    {
        public static readonly TemplateHistory SingObj = new TemplateHistory();

        private bool _canBack = false;
        public bool CanBack
        {
            get { return _canBack; }
            set
            {
                _canBack = value;
                RaisePropertyChanged("CanBack");
            }
        }
        private Stack<KeyValuePair<string, XElement>> _history = new Stack<KeyValuePair<string, XElement>>();
        private KeyValuePair<string, XElement> _default = new KeyValuePair<string, XElement>();
        public void Add(string templateName, XElement templateXml)
        {
            _history.Push(new KeyValuePair<string, XElement>(templateName, templateXml));
            _canBack = true;
        }

        public KeyValuePair<string, XElement> Back()
        {
            if (_history.Any())
                return _history.Pop();
            else
            {
                _canBack = false;
                return _default;
            }
        }
    }
}
