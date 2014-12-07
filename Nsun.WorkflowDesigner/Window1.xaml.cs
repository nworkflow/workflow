using Nsun.Tools.WorkflowDesigner.Controls;
using Nsun.Tools.WorkflowDesigner.Controls.Tools;
using Nsun.Workflow.Core.EnumExt;
using System.Windows;
using System.Windows.Controls;
using WPG;
using Xceed.Wpf.AvalonDock.Layout;

namespace DiagramDesigner
{
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            Loaded += (s, r) =>
            {
                (this.FindName("MyDesigner") as DesignerCanvas).AssociateProperty = this.FindName("propertyGrid") as PropertyGrid;
                zoomBox.ScrollViewer = DesignerScrollViewer;

                Toolbox toolBox = new Toolbox();
                toolBox.Items.Add(new ToolBoxItemExt("标准节点", "起始节点", string.Empty, ActivityTypeEnum.Start.ToString()));
                toolBox.Items.Add(new ToolBoxItemExt("标准节点", "标准节点", string.Empty, ActivityTypeEnum.Process.ToString()));
                toolBox.Items.Add(new ToolBoxItemExt("标准节点", "条件节点", string.Empty, ActivityTypeEnum.Decision.ToString()));
                toolBox.Items.Add(new ToolBoxItemExt("标准节点", "并发结束", string.Empty, ActivityTypeEnum.Parallel.ToString()));

                toolBox.Items.Add(new ToolBoxItemExt("扩展节点", "子流程", string.Empty, ActivityTypeEnum.SubRouting.ToString()));
                toolBox.Items.Add(new ToolBoxItemExt("扩展节点", "多子流程", string.Empty, ActivityTypeEnum.SubRoutings.ToString()));
                toolBox.Items.Add(new ToolBoxItemExt("扩展节点", "消息", string.Empty, ActivityTypeEnum.NotifyMsg.ToString()));
                this.expBookmark.Content = toolBox;
                this.DataContext = this;
            };
        }

        LayoutDocument debuging = null;
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            switch (mi.Name)
            {
                case "miShowToolBar":
                    this.toolBar.Visibility = Visibility.Visible;
                    break;
                case "miHidenToolBar":
                    this.toolBar.Visibility = Visibility.Collapsed;
                    break;
                case "miDebuging":
                    if (debuging == null)
                    {
                        debuging = new LayoutDocument();
                        debuging.Title = "调试";
                        debuging.Content = new Debuging();
                        layoutDocumenPane.Children.Insert(layoutDocumenPane.ChildrenCount-1,debuging);
                    }
                    layoutDocumenPane.SelectedContentIndex = layoutDocumenPane.IndexOfChild(debuging);
                    break;
             }
        }

        private bool _toolBarVisible = true;
        private void CommandBinding_Executed_ShowToolBar(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            this.toolBar.Visibility = Visibility.Visible;
        }

        private void CommandBinding_Executed_HidenToolBar(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            _toolBarVisible = !_toolBarVisible;
            this.toolBar.Visibility = _toolBarVisible ? Visibility.Visible : Visibility.Collapsed ;
        }

        private void NewLayoutDocument_IsSelectChange(object sender, System.EventArgs e)
        {
            LayoutDocument layoutDocument = sender as LayoutDocument;
            if (layoutDocument.ContentId == "code" && layoutDocument.IsSelected)
            {
                try
                {
                    txtCode.Text = MyDesigner.TemplateCode;
                }
                catch (System.Exception)
                {
                }
            }
        }

    }
}
