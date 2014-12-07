using Nsun.Tools.WorkflowDesigner.Views;
using Nsun.Workflow.Core.EnumExt;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nsun.Tools.WorkflowDesigner.Controls.Tools
{
    /// <summary>
    /// Debuging.xaml 的互動邏輯
    /// </summary>
    public partial class Debuging : UserControl
    {
        private Workflow.SerContainer.WFService.Service1Client _client = new Workflow.SerContainer.WFService.Service1Client();
        public Debuging()
        {
            InitializeComponent();
            Loaded += (s, r) =>
            {
                List<string> submitEnumInfos = new List<string>();
                var submitEnums = typeof(SubmitTypeEnum).GetEnumNames().ToList();
                submitEnums.ForEach(p =>
                {
                    submitEnumInfos.Add(p);
                });
                this.cmbSubmitType.ItemsSource = submitEnumInfos;
                this.grdOtherInfo.ItemsSource = new List<NSRoutingData>();
            };
        }


        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (this.lstNodeInfos.SelectedItem != null)
            {
                var item = this.lstNodeInfos.SelectedItem as NSNodeInfo;
                if (item != null)
                {
                    if (this.cmbSubmitType.SelectedIndex > -1 && ((this.cmbSubmitType.SelectedValue.ToString() == SubmitTypeEnum._BK.ToString()) ? this.cmbBacknodes.SelectedIndex > -1 : true))
                    {
                        SubmitInfo_DTO submitInfo = new SubmitInfo_DTO();
                        submitInfo.NodeId = item.Id;
                        submitInfo.Condition = this.txtSubmit.Text;
                        submitInfo.ExpandDatas = new List<ExpandData>() { new ExpandData() };
                        submitInfo.SubmitResult = this.cmbSubmitType.SelectedValue.ToString() + ((this.cmbSubmitType.SelectedValue.ToString() == SubmitTypeEnum._BK.ToString()) ? ":"
                            + (this.cmbBacknodes.SelectedValue as InstanceNode_DTO).Name : "");
                        this.txbMsg.Text = _client.SubmitWorkflow(submitInfo);
                        AddNewChild(item.NodeName);
                    }
                    else
                        this.txbMsg.Text = "提交值出错";
                }
            }
        }


        private void btnTemplateName_Click(object sender, RoutedEventArgs e)
        {
            var templateList = new TemplateInfoManager();
            templateList.Closed += (s, r) =>
            {
                if (templateList.SelectTemplateInfo != null)
                {
                    this.txtTemplateName.Text = templateList.SelectTemplateInfo.TemplateName;
                    this.txtTemplateName.Tag = templateList.SelectTemplateInfo.Id;

                    // 获取该模板下的所有节点
                    var nodes = _client.GetNodesByTemplateId(templateList.SelectTemplateInfo.Id);
                    this.cmbBacknodes.ItemsSource = nodes;
                    this.cmbBacknodes.DisplayMemberPath = "Name";
                }
            };

            templateList.Show();
        }


        private void btnStartWorkflow_Click(object sender, RoutedEventArgs e)
        {
            this.txtDebugInfo.Text = string.Empty;
            if (string.IsNullOrEmpty(this.txtTemplateName.Text))
                return;
            string errorInfo = string.Empty;
            Guid taskId = _client.StartWorkflowInfo(new Workflow.SerContainer.WFService.StartInfo_DTO()
            {
                TemplateId = (Guid)this.txtTemplateName.Tag,
                TemplateName = this.txtTemplateName.Text,
                TaskName = "测试",
                TaskType = "测试"
            }, ref errorInfo);

            if (!string.IsNullOrEmpty(errorInfo))
                this.txbMsg.Text = errorInfo;
            else
            {
                this.txtTaskId.Text = taskId.ToString();
                this.txbMsg.Text = "启动成功！";
                this.txtDebugInfo.Text = "开始";
            }
        }


        private void AddNewChild(string msg)
        {
            this.txtDebugInfo.Text +="---"+ msg;
        }

        private void btnGetCurrentNode_Click(object sender, RoutedEventArgs e)
        {
            if (this.lstInstanceInfos.SelectedItem != null)
            {
                var selectItem = this.lstInstanceInfos.SelectedItem as NSInstanceInfo;
                if (selectItem != null)
                {
                    this.lstNodeInfos.ItemsSource = _client.GetNSNodeInfosByInstanceId(selectItem.Id);
                }
            }
        }


        private void btnGetInstances_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtTaskId.Text))
            {
                this.lstInstanceInfos.ItemsSource = _client.GetNSInfoInfosByTaskId(Guid.Parse(this.txtTaskId.Text));
            }
            else
            {
                this.txbMsg.Text = "选择任务！";
            }
        }
    }
}
