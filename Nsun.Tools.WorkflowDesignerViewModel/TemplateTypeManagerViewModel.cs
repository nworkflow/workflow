using Nsun.Framework;
using Nsun.NSFramework;
using Nsun.NSFramework.Helper;
using Nsun.Workflow.SerContainer.WFService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Nsun.Tools.WorkflowDesignerViewModel
{
    public class TemplateTypeManagerViewModel : ViewModelBase
    {
        public ICommand AddNewTemplateTypeCommand
        {
            get;
            private set;
        }


        public ICommand DelTemplateTypeCommand
        {
            get;
            private set;
        }


        public ICommand SaveTemplateTypeCommand
        {
            get;
            private set;
        }

        private string _typeName;
        public string TypeName
        {
            get { return _typeName; }
            set
            {
                _typeName = value;
                RaisePropertyChanged("TypeName");
            }
        }

        private string _typeDes;
        public string TypeDes
        {
            get { return _typeDes; }
            set
            {
                _typeDes = value;
                RaisePropertyChanged("TypeDes");
            }
        }


        private EntityCheckedHelperGeneric<NSTemplateType> _nsTemplateTypeSelect;
        public EntityCheckedHelperGeneric<NSTemplateType> NsTemplateTypeSelect
        {
            get { return _nsTemplateTypeSelect; }
            set
            {
                _nsTemplateTypeSelect = value;
                RaisePropertyChanged("NsTemplateTypeSelect");
            }
        }


        private ObservableCollection<EntityCheckedHelperGeneric<NSTemplateType>> _nsTemplateTypes;
        public ObservableCollection<EntityCheckedHelperGeneric<NSTemplateType>> NSTemplateTypes
        {
            get { return _nsTemplateTypes; }
            set
            {
                if (_nsTemplateTypes != value)
                {
                    _nsTemplateTypes = value;
                    RaisePropertyChanged("NSTemplateTypes");
                }
            }
        }


        public TemplateTypeManagerViewModel()
        {
            AddNewTemplateTypeCommand = new RelayCommand(AddNewTemplateTypeExecute);
            DelTemplateTypeCommand = new RelayCommand(DelTemplateTypeExecute);
            SaveTemplateTypeCommand = new RelayCommand(SaveTemplateTypeExecute);
            NsTemplateTypeSelect = new EntityCheckedHelperGeneric<NSTemplateType>(new NSTemplateType());
            NSTemplateTypes = new ObservableCollection<EntityCheckedHelperGeneric<NSTemplateType>>();
            var templateTypes =  _client.GetAllTemplateType();
            foreach (var item in templateTypes)
            {
                EntityCheckedHelperGeneric<NSTemplateType> entity = new EntityCheckedHelperGeneric<NSTemplateType>(item);
                this.NSTemplateTypes.Add(entity);
            }
        }


        public void AddNewTemplateTypeExecute()
        {
            if (string.IsNullOrEmpty(NsTemplateTypeSelect.TEntity.TemplateType) || string.IsNullOrEmpty(NsTemplateTypeSelect.TEntity.TemplateDes))
            {
                ShowMessage("类型名和类型描述不能为空！");
                return;
            }
            var item = this.NSTemplateTypes.Where(p => p.TEntity.TemplateType == NsTemplateTypeSelect.TEntity.TemplateType).FirstOrDefault();
            if (item != null)
            {
                ShowMessage("不能添加重复类型名称！");
                return;
            }
            
            NSTemplateType templateType = new NSTemplateType();
            templateType.TemplateType =NsTemplateTypeSelect.TEntity.TemplateType;
            templateType.TemplateDes =NsTemplateTypeSelect.TEntity.TemplateDes;
            templateType.Id = Guid.NewGuid();
            templateType.Status = -1;
           
            this.NSTemplateTypes.Add(new EntityCheckedHelperGeneric<NSTemplateType>(templateType));
        }


        private Workflow.SerContainer.WFService.Service1Client _client = new Workflow.SerContainer.WFService.Service1Client();
        public void DelTemplateTypeExecute()
        {
            var delItems = this.NSTemplateTypes.Where(p => p.IsChecked);
            var delItemDTO =delItems.Select(p=>p.TEntity).ToList<NSTemplateType>();

            string result =  _client.DelTemplateTypes(delItemDTO);
            if (string.IsNullOrEmpty(result))
            {
                foreach (var item in delItems)
                {
                    NSTemplateTypes.Remove(item);
                }
                ShowMessage("保存成功！");
            }
            else
            {
                ShowMessage(result);
            }
        }


        public void SaveTemplateTypeExecute()
        {
            var item = this.NSTemplateTypes.Where(p => p.TEntity.Status == -1).Select(p => p.TEntity).ToList();
            item.ForEach(p => p.Status = 0);
            
            string result = _client.NewTemplateTypes(item.ToList());
            ShowMessage(result);
        }


        private void ShowMessage(string result)
        {
            if (string.IsNullOrEmpty(result))
            {
                MessageHelper.ShowMessageBox("保存成功！");
            }
            else
            {
                MessageHelper.ShowMessageBox(result);
            }
        }
    }
}
