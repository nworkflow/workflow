using Nsun.Framework;
using Nsun.NSFramework;
using Nsun.NSFramework.Helper;
using Nsun.Workflow.SerContainer.WFService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Nsun.Tools.WorkflowDesignerViewModel
{
    public class TemplateInfoManagerViewModel : ViewModelBase
    {

        private Workflow.SerContainer.WFService.Service1Client _client ;
        public TemplateInfoManagerViewModel()
        {
            try
            {
                _client = new Workflow.SerContainer.WFService.Service1Client();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessageBox(ex.ToString());
            }
           
            NsTemplateTypes = new ObservableCollection<EntityCheckedHelperGeneric<NSTemplateType>>();
            var templateTypes = _client.GetAllTemplateType();
            foreach (var item in templateTypes)
            {
                EntityCheckedHelperGeneric<NSTemplateType> entity = new EntityCheckedHelperGeneric<NSTemplateType>(item);
                this.NsTemplateTypes.Add(entity);
            }
        }


        ObservableCollection<EntityCheckedHelperGeneric<NSTemplateInfo>> _nsTemplateInfos=new ObservableCollection<EntityCheckedHelperGeneric<NSTemplateInfo>>();
        public ObservableCollection<EntityCheckedHelperGeneric<NSTemplateInfo>> NsTemplateInfos
        {
            get { return _nsTemplateInfos; }
            set
            {
                _nsTemplateInfos = value;
                RaisePropertyChanged("NsTemplateInfos");
            }
        }

        ObservableCollection<EntityCheckedHelperGeneric<NSTemplateType>> _nsTemplateTypes;
        public ObservableCollection<EntityCheckedHelperGeneric<NSTemplateType>> NsTemplateTypes
        {
            get { return _nsTemplateTypes; }
            set
            {
                _nsTemplateTypes = value;
                RaisePropertyChanged("NsTemplateTypes");
            }
        }


        public void GetTemplatesByType(string templateType)
        {
            if (string.IsNullOrEmpty(templateType))
            {
                MessageHelper.ShowMessageBox("请选择模板类型！");
                return;
            }

            var templateInfos = _client.GetAllTemplateInfoByType(templateType);
            NsTemplateInfos.Clear();
            foreach (var item in templateInfos)
            {
                EntityCheckedHelperGeneric<NSTemplateInfo> entity = new EntityCheckedHelperGeneric<NSTemplateInfo>(item);
                NsTemplateInfos.Add(entity);
            }
        }
    }
}
