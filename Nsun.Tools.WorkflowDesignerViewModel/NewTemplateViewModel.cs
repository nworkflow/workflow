using Nsun.NSFramework;
using Nsun.Workflow.SerContainer.WFService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Nsun.Tools.WorkflowDesignerViewModel
{
    public class NewTemplateViewModel : ViewModelBase
    {
        public ICommand SaveCommand
        {
            get;
            private set;
        }

        private string _templateName;
        public string TemplateName
        {
            get { return _templateName; }
            set
            {
                _templateName = value;
                RaisePropertyChanged("TemplateName");
            }
        }


        private NSTemplateType _selectType;
        public NSTemplateType SelectType
        {
            get { return _selectType; }
            set
            {
                _selectType = value;
                RaisePropertyChanged("SelectType");
            }
        }

        public string TemplateXML { get; set; }

        private ObservableCollection<NSTemplateType> _templateTypes;
        public ObservableCollection<NSTemplateType> TemplateTypes
        {
            get { return _templateTypes; }
            set
            {
                _templateTypes = value;
                RaisePropertyChanged("TemplateTypes");
            }
        }


        private Workflow.SerContainer.WFService.Service1Client _client = new Workflow.SerContainer.WFService.Service1Client();
        public NewTemplateViewModel()
        {
            SaveCommand = new RelayCommand(SaveExecute);
            var templateTypes = _client.GetAllTemplateType();
            TemplateTypes = new ObservableCollection<NSTemplateType>(templateTypes);
        }


        private void SaveExecute()
        {
            if (SelectType == null)
            {
                MessageHelper.ShowMessageBox("请选择类型！");
                return;
            }

            NSTemplateInfo template = new NSTemplateInfo();
            template.Id = Guid.NewGuid();
            template.TemplateName = TemplateName;
            template.TemplateText = TemplateXML;
            template.TemplateType = SelectType.TemplateType;
            string msg = _client.NewTemplate(template);
            if (string.IsNullOrEmpty(msg))
            {
                MessageHelper.ShowMessageBox("保存成功！");
                if (WindowAction != null)
                    WindowAction();
            }
            else
                MessageHelper.ShowMessageBox(msg);
        }
    }
}
