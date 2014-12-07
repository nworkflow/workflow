using Nsun.NSFramework;
using Nsun.NSFramework.Helper;
using Nsun.Tools.WorkflowDesignerViewModel;
using Nsun.Workflow.SerContainer.WFService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Nsun.Tools.WorkflowDesigner.Views
{
    /// <summary>
    /// TemplateInfoManager.xaml 的互動邏輯
    /// </summary>
    public partial class TemplateInfoManager : Window
    {
        TemplateInfoManagerViewModel vm;
        public NSTemplateInfo SelectTemplateInfo
        {
            get;
            private set;
        }

        public bool? DialogResultExt = null;

        public TemplateInfoManager()
        {
            InitializeComponent();
            vm = Resources["vm"] as TemplateInfoManagerViewModel;
        }


        private void tvTemplateType_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var item =  e.NewValue as EntityCheckedHelperGeneric<NSTemplateType>;
            if (item == null)
                return;
            vm.GetTemplatesByType(item.TEntity.TemplateType);
        }

        private void btnSelectTemplate_Click(object sender, RoutedEventArgs e)
        {
            var templateInfo = grdTemplates.SelectedItem as EntityCheckedHelperGeneric<NSTemplateInfo>;
            if (templateInfo != null)
            {
                this.SelectTemplateInfo = templateInfo.TEntity;
                DialogResultExt = true;
                this.Close();
            }
            else
            {
                MessageHelper.ShowMessageBox("请选择要调试的模板！");
            }
        }
    }
}
