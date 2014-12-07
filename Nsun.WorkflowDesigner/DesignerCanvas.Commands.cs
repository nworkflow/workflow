using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Win32;
using Nsun.Tools.Common;
using System.Text;
using DiagramDesigner.Views;
using Nsun.Workflow.SerContainer.WFService;
using Nsun.Tools.WorkflowDesigner.Views;
using Nsun.NSFramework;
using Nsun.Workflow.Core;
using Nsun.Workflow.Core.Models;
using Nsun.Workflow.Core.EnumExt;
using Nsun.Workflow.Core.Activities;
using Nsun.Workflow.Core.Utils;
using Nsun.Tools.WorkflowDesigner.Utils;
using Nsun.Workflow.Core.Routing;

namespace DiagramDesigner
{
    public partial class DesignerCanvas
    {
        public static RoutedCommand Group = new RoutedCommand();
        public static RoutedCommand Ungroup = new RoutedCommand();
        public static RoutedCommand BringForward = new RoutedCommand();
        public static RoutedCommand BringToFront = new RoutedCommand();
        public static RoutedCommand SendBackward = new RoutedCommand();
        public static RoutedCommand SendToBack = new RoutedCommand();
        public static RoutedCommand AlignTop = new RoutedCommand();
        public static RoutedCommand AlignVerticalCenters = new RoutedCommand();
        public static RoutedCommand AlignBottom = new RoutedCommand();
        public static RoutedCommand AlignLeft = new RoutedCommand();
        public static RoutedCommand AlignHorizontalCenters = new RoutedCommand();
        public static RoutedCommand AlignRight = new RoutedCommand();
        public static RoutedCommand DistributeHorizontal = new RoutedCommand();
        public static RoutedCommand DistributeVertical = new RoutedCommand();
        public static RoutedCommand SelectAll = new RoutedCommand();
        public static RoutedCommand Details = new RoutedCommand();
        public static RoutedCommand SaveTemplateToDataBase = new RoutedCommand();
        public static RoutedCommand AddTemplateType = new RoutedCommand();
        public static RoutedCommand ManagerTemplateType = new RoutedCommand();
        public static RoutedCommand DebugTemplate = new RoutedCommand();
        public static RoutedCommand OpenFromDataBase = new RoutedCommand();
        public static RoutedCommand SettingCondition = new RoutedCommand();
        public static RoutedCommand Validate = new RoutedCommand();
        public static RoutedCommand GetTemplateInfo = new RoutedCommand();
        public static RoutedCommand GetTemplateXml = new RoutedCommand();
        public static RoutedCommand ParallNode = new RoutedCommand();
        public static RoutedCommand LinkChildTemplate = new RoutedCommand();
        public static RoutedCommand HisTemplate = new RoutedCommand();
        public DesignerCanvas()
        {
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.New, New_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, Open_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, Save_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Print, Print_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, Cut_Executed, Cut_Enabled));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, Copy_Executed, Copy_Enabled));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, Paste_Executed, Paste_Enabled));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, Delete_Executed, Delete_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.Group, Group_Executed, Group_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.Ungroup, Ungroup_Executed, Ungroup_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.BringForward, BringForward_Executed, Order_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.BringToFront, BringToFront_Executed, Order_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.SendBackward, SendBackward_Executed, Order_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.SendToBack, SendToBack_Executed, Order_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.AlignTop, AlignTop_Executed, Align_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.AlignVerticalCenters, AlignVerticalCenters_Executed, Align_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.AlignBottom, AlignBottom_Executed, Align_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.AlignLeft, AlignLeft_Executed, Align_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.AlignHorizontalCenters, AlignHorizontalCenters_Executed, Align_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.AlignRight, AlignRight_Executed, Align_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.DistributeHorizontal, DistributeHorizontal_Executed, Distribute_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.DistributeVertical, DistributeVertical_Executed, Distribute_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.SelectAll, SelectAll_Executed));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.Details, Details_Executed));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.SaveTemplateToDataBase, SaveTemplateToDataBase_Executed));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.AddTemplateType, AddTemplateType_Executed));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.ManagerTemplateType, ManagerTemplateType_Executed));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.DebugTemplate, DebugTemplate_Executed));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.OpenFromDataBase, OpenFromDataBase_Execute));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.SettingCondition, SettingCondition_Execute));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.Validate, Validate_Excecute));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.GetTemplateInfo, GetTemplateInfo_Execute, CanGetTemplateInfo_Execute));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.ParallNode, SettingParallNode_Excute, CanSettingParallelNode_Exectue));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.LinkChildTemplate, LinkChildTemplate_Execute, CanGetTemplateInfo_Execute));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.HisTemplate, HisTemplate_Execute, CanHisTemplate_Execute));
            SelectAll.InputGestures.Add(new KeyGesture(Key.A, ModifierKeys.Control));

            this.AllowDrop = true;
            Clipboard.Clear();
        }

        #region New Command

        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            InitTemplateBase();
            this.SelectionService.ClearSelection();
        }

        #endregion

        #region Open Command

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            XElement root = LoadSerializedDataFromFile();

            if (root == null)
                return;

            InitTemplate(root);
        }

        #endregion

        #region Save Command

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IEnumerable<DesignerItem> designerItems = this.Children.OfType<DesignerItem>();
            IEnumerable<Connection> connections = this.Children.OfType<Connection>();

            string designerItemShape = SerializeDesignerShape(designerItems);
            XElement designerItemsXML = SerializeDesignerItems(designerItems);
            XElement connectionsXML = SerializeConnections(connections);

            XElement root = new XElement("Root");
            root.Add(designerItemsXML);
            root.Add(connectionsXML);

            SaveFile(root);
        }



        #endregion

        #region Print Command

        private void Print_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SelectionService.ClearSelection();

            PrintDialog printDialog = new PrintDialog();

            if (true == printDialog.ShowDialog())
            {
                printDialog.PrintVisual(this, "WPF Diagram");
            }
        }

        #endregion

        #region Copy Command

        private void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CopyCurrentSelection();
        }

        private void Copy_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectionService.CurrentSelection.Count() > 0;
        }

        #endregion

        #region Paste Command

        private void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            XElement root = LoadSerializedDataFromClipBoard();

            if (root == null)
                return;

            // create DesignerItems
            Dictionary<Guid, Guid> mappingOldToNewIDs = new Dictionary<Guid, Guid>();
            List<ISelectable> newItems = new List<ISelectable>();
            IEnumerable<XElement> itemsXML = root.Elements("DesignerItems").Elements("DesignerItem");

            double offsetX = Double.Parse(root.Attribute("OffsetX").Value, CultureInfo.InvariantCulture);
            double offsetY = Double.Parse(root.Attribute("OffsetY").Value, CultureInfo.InvariantCulture);

            foreach (XElement itemXML in itemsXML)
            {
                Guid oldID = new Guid(itemXML.Element("ID").Value);
                Guid newID = Guid.NewGuid();
                mappingOldToNewIDs.Add(oldID, newID);
                DesignerItem item = DeserializeDesignerItem(itemXML, newID, offsetX, offsetY);
                this.Children.Add(item);
                SetConnectorDecoratorTemplate(item);
                newItems.Add(item);
            }

            // update group hierarchy
            SelectionService.ClearSelection();
            foreach (DesignerItem el in newItems)
            {
                if (el.ParentID != Guid.Empty)
                    el.ParentID = mappingOldToNewIDs[el.ParentID];
            }


            foreach (DesignerItem item in newItems)
            {
                if (item.ParentID == Guid.Empty)
                {
                    SelectionService.AddToSelection(item);
                }
            }

            // create Connections
            IEnumerable<XElement> connectionsXML = root.Elements("Connections").Elements("Connection");
            foreach (XElement connectionXML in connectionsXML)
            {
                Guid oldSourceID = new Guid(connectionXML.Element("SourceID").Value);
                Guid oldSinkID = new Guid(connectionXML.Element("SinkID").Value);

                if (mappingOldToNewIDs.ContainsKey(oldSourceID) && mappingOldToNewIDs.ContainsKey(oldSinkID))
                {
                    Guid newSourceID = mappingOldToNewIDs[oldSourceID];
                    Guid newSinkID = mappingOldToNewIDs[oldSinkID];

                    String sourceConnectorName = connectionXML.Element("SourceConnectorName").Value;
                    String sinkConnectorName = connectionXML.Element("SinkConnectorName").Value;

                    Connector sourceConnector = GetConnector(newSourceID, sourceConnectorName);
                    Connector sinkConnector = GetConnector(newSinkID, sinkConnectorName);

                    Connection connection = new Connection(sourceConnector, sinkConnector);
                    connection.Condition.Condition = connectionXML.Element("Condition").Value;
                    Canvas.SetZIndex(connection, Int32.Parse(connectionXML.Element("zIndex").Value));
                    this.Children.Add(connection);

                    SelectionService.AddToSelection(connection);
                }
            }

            DesignerCanvas.BringToFront.Execute(null, this);

            // update paste offset
            root.Attribute("OffsetX").Value = (offsetX + 10).ToString();
            root.Attribute("OffsetY").Value = (offsetY + 10).ToString();
            Clipboard.Clear();
            Clipboard.SetData(DataFormats.Xaml, root);
        }

        private void Paste_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Clipboard.ContainsData(DataFormats.Xaml);
        }

        #endregion

        #region Delete Command

        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DeleteCurrentSelection();
        }

        private void Delete_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.SelectionService.CurrentSelection.Count() > 0;
        }

        #endregion

        #region Cut Command

        private void Cut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CopyCurrentSelection();
            DeleteCurrentSelection();
        }

        private void Cut_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.SelectionService.CurrentSelection.Count() > 0;
        }

        #endregion

        #region Group Command

        private void Group_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var items = from item in this.SelectionService.CurrentSelection.OfType<DesignerItem>()
                        where item.ParentID == Guid.Empty
                        select item;

            Rect rect = GetBoundingRectangle(items);

            DesignerItem groupItem = new DesignerItem();
            groupItem.IsGroup = true;
            groupItem.Width = rect.Width;
            groupItem.Height = rect.Height;
            Canvas.SetLeft(groupItem, rect.Left);
            Canvas.SetTop(groupItem, rect.Top);
            Canvas groupCanvas = new Canvas();
            groupItem.Content = groupCanvas;
            Canvas.SetZIndex(groupItem, this.Children.Count);
            this.Children.Add(groupItem);

            foreach (DesignerItem item in items)
                item.ParentID = groupItem.ID;

            this.SelectionService.SelectItem(groupItem);
        }

        private void Group_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            int count = (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                         where item.ParentID == Guid.Empty
                         select item).Count();

            e.CanExecute = count > 1;
        }

        #endregion

        #region Ungroup Command

        private void Ungroup_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var groups = (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                          where item.IsGroup && item.ParentID == Guid.Empty
                          select item).ToArray();

            foreach (DesignerItem groupRoot in groups)
            {
                var children = from child in SelectionService.CurrentSelection.OfType<DesignerItem>()
                               where child.ParentID == groupRoot.ID
                               select child;

                foreach (DesignerItem child in children)
                    child.ParentID = Guid.Empty;

                this.SelectionService.RemoveFromSelection(groupRoot);
                this.Children.Remove(groupRoot);
                UpdateZIndex();
            }
        }

        private void Ungroup_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            var groupedItem = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                              where item.ParentID != Guid.Empty
                              select item;


            e.CanExecute = groupedItem.Count() > 0;
        }

        #endregion

        #region BringForward Command

        private void BringForward_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            List<UIElement> ordered = (from item in SelectionService.CurrentSelection
                                       orderby Canvas.GetZIndex(item as UIElement) descending
                                       select item as UIElement).ToList();

            int count = this.Children.Count;

            for (int i = 0; i < ordered.Count; i++)
            {
                int currentIndex = Canvas.GetZIndex(ordered[i]);
                int newIndex = Math.Min(count - 1 - i, currentIndex + 1);
                if (currentIndex != newIndex)
                {
                    Canvas.SetZIndex(ordered[i], newIndex);
                    IEnumerable<UIElement> it = this.Children.OfType<UIElement>().Where(item => Canvas.GetZIndex(item) == newIndex);

                    foreach (UIElement elm in it)
                    {
                        if (elm != ordered[i])
                        {
                            Canvas.SetZIndex(elm, currentIndex);
                            break;
                        }
                    }
                }
            }
        }

        private void Order_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            //e.CanExecute = SelectionService.CurrentSelection.Count() > 0;
            e.CanExecute = true;
        }

        #endregion

        #region BringToFront Command

        private void BringToFront_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            List<UIElement> selectionSorted = (from item in SelectionService.CurrentSelection
                                               orderby Canvas.GetZIndex(item as UIElement) ascending
                                               select item as UIElement).ToList();

            List<UIElement> childrenSorted = (from UIElement item in this.Children
                                              orderby Canvas.GetZIndex(item as UIElement) ascending
                                              select item as UIElement).ToList();

            int i = 0;
            int j = 0;
            foreach (UIElement item in childrenSorted)
            {
                if (selectionSorted.Contains(item))
                {
                    int idx = Canvas.GetZIndex(item);
                    Canvas.SetZIndex(item, childrenSorted.Count - selectionSorted.Count + j++);
                }
                else
                {
                    Canvas.SetZIndex(item, i++);
                }
            }
        }

        #endregion

        #region SendBackward Command

        private void SendBackward_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            List<UIElement> ordered = (from item in SelectionService.CurrentSelection
                                       orderby Canvas.GetZIndex(item as UIElement) ascending
                                       select item as UIElement).ToList();

            int count = this.Children.Count;

            for (int i = 0; i < ordered.Count; i++)
            {
                int currentIndex = Canvas.GetZIndex(ordered[i]);
                int newIndex = Math.Max(i, currentIndex - 1);
                if (currentIndex != newIndex)
                {
                    Canvas.SetZIndex(ordered[i], newIndex);
                    IEnumerable<UIElement> it = this.Children.OfType<UIElement>().Where(item => Canvas.GetZIndex(item) == newIndex);

                    foreach (UIElement elm in it)
                    {
                        if (elm != ordered[i])
                        {
                            Canvas.SetZIndex(elm, currentIndex);
                            break;
                        }
                    }
                }
            }
        }

        #endregion

        #region SendToBack Command

        private void SendToBack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            List<UIElement> selectionSorted = (from item in SelectionService.CurrentSelection
                                               orderby Canvas.GetZIndex(item as UIElement) ascending
                                               select item as UIElement).ToList();

            List<UIElement> childrenSorted = (from UIElement item in this.Children
                                              orderby Canvas.GetZIndex(item as UIElement) ascending
                                              select item as UIElement).ToList();
            int i = 0;
            int j = 0;
            foreach (UIElement item in childrenSorted)
            {
                if (selectionSorted.Contains(item))
                {
                    int idx = Canvas.GetZIndex(item);
                    Canvas.SetZIndex(item, j++);

                }
                else
                {
                    Canvas.SetZIndex(item, selectionSorted.Count + i++);
                }
            }
        }

        #endregion

        #region AlignTop Command

        private void AlignTop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                double top = Canvas.GetTop(selectedItems.First());

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = top - Canvas.GetTop(item);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        Canvas.SetTop(di, Canvas.GetTop(di) + delta);
                    }
                }
            }
        }

        private void Align_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            //var groupedItem = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
            //                  where item.ParentID == Guid.Empty
            //                  select item;


            //e.CanExecute = groupedItem.Count() > 1;
            e.CanExecute = true;
        }

        #endregion

        #region AlignVerticalCenters Command

        private void AlignVerticalCenters_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                double bottom = Canvas.GetTop(selectedItems.First()) + selectedItems.First().Height / 2;

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = bottom - (Canvas.GetTop(item) + item.Height / 2);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        Canvas.SetTop(di, Canvas.GetTop(di) + delta);
                    }
                }
            }
        }

        #endregion

        #region AlignBottom Command

        private void AlignBottom_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                double bottom = Canvas.GetTop(selectedItems.First()) + selectedItems.First().Height;

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = bottom - (Canvas.GetTop(item) + item.Height);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        Canvas.SetTop(di, Canvas.GetTop(di) + delta);
                    }
                }
            }
        }

        #endregion

        #region AlignLeft Command

        private void AlignLeft_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                double left = Canvas.GetLeft(selectedItems.First());

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = left - Canvas.GetLeft(item);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        Canvas.SetLeft(di, Canvas.GetLeft(di) + delta);
                    }
                }
            }
        }

        #endregion

        #region AlignHorizontalCenters Command

        private void AlignHorizontalCenters_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                double center = Canvas.GetLeft(selectedItems.First()) + selectedItems.First().Width / 2;

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = center - (Canvas.GetLeft(item) + item.Width / 2);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        Canvas.SetLeft(di, Canvas.GetLeft(di) + delta);
                    }
                }
            }
        }

        #endregion

        #region AlignRight Command

        private void AlignRight_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                double right = Canvas.GetLeft(selectedItems.First()) + selectedItems.First().Width;

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = right - (Canvas.GetLeft(item) + item.Width);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        Canvas.SetLeft(di, Canvas.GetLeft(di) + delta);
                    }
                }
            }
        }

        #endregion

        #region DistributeHorizontal Command

        private void DistributeHorizontal_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                let itemLeft = Canvas.GetLeft(item)
                                orderby itemLeft
                                select item;

            if (selectedItems.Count() > 1)
            {
                double left = Double.MaxValue;
                double right = Double.MinValue;
                double sumWidth = 0;
                foreach (DesignerItem item in selectedItems)
                {
                    left = Math.Min(left, Canvas.GetLeft(item));
                    right = Math.Max(right, Canvas.GetLeft(item) + item.Width);
                    sumWidth += item.Width;
                }

                double distance = Math.Max(0, (right - left - sumWidth) / (selectedItems.Count() - 1));
                double offset = Canvas.GetLeft(selectedItems.First());

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = offset - Canvas.GetLeft(item);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        Canvas.SetLeft(di, Canvas.GetLeft(di) + delta);
                    }
                    offset = offset + item.Width + distance;
                }
            }
        }

        private void Distribute_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            //var groupedItem = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
            //                  where item.ParentID == Guid.Empty
            //                  select item;


            //e.CanExecute = groupedItem.Count() > 1;
            e.CanExecute = true;
        }

        #endregion

        #region DistributeVertical Command

        private void DistributeVertical_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                let itemTop = Canvas.GetTop(item)
                                orderby itemTop
                                select item;

            if (selectedItems.Count() > 1)
            {
                double top = Double.MaxValue;
                double bottom = Double.MinValue;
                double sumHeight = 0;
                foreach (DesignerItem item in selectedItems)
                {
                    top = Math.Min(top, Canvas.GetTop(item));
                    bottom = Math.Max(bottom, Canvas.GetTop(item) + item.Height);
                    sumHeight += item.Height;
                }

                double distance = Math.Max(0, (bottom - top - sumHeight) / (selectedItems.Count() - 1));
                double offset = Canvas.GetTop(selectedItems.First());

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = offset - Canvas.GetTop(item);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        Canvas.SetTop(di, Canvas.GetTop(di) + delta);
                    }
                    offset = offset + item.Height + distance;
                }
            }
        }

        #endregion

        #region SelectAll Command

        private void SelectAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SelectionService.SelectAll();
        }

        #endregion

        #region Helper Methods

        private XElement LoadSerializedDataFromFile()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Designer Files (*.xml)|*.xml|All Files (*.*)|*.*";

            if (openFile.ShowDialog() == true)
            {
                try
                {
                    return XElement.Load(openFile.FileName);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.StackTrace, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return null;
        }

        void SaveFile(XElement xElement)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Files (*.xml)|*.xml|All Files (*.*)|*.*";
            if (saveFile.ShowDialog() == true)
            {
                try
                {
                    xElement.Save(saveFile.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private XElement LoadSerializedDataFromClipBoard()
        {
            if (Clipboard.ContainsData(DataFormats.Xaml))
            {
                String clipboardData = Clipboard.GetData(DataFormats.Xaml) as String;

                if (String.IsNullOrEmpty(clipboardData))
                    return null;
                try
                {
                    return XElement.Load(new StringReader(clipboardData));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.StackTrace, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return null;
        }

        private XElement SerializeDesignerItems(IEnumerable<DesignerItem> designerItems)
        {
            XElement serializedItems = new XElement("DesignerItems",
                                       from item in designerItems
                                       let contentXaml = XamlWriter.Save(((DesignerItem)item).Content)
                                       select new XElement("DesignerItem",
                                                  new XElement("Name", item.DisplayName),
                                                  new XElement("Type", item.GetType()),
                                                  new XElement("BType", ((DesignerItem)item).BookmarkBase.NodeType),
                                                  new XElement("GroupName", ((DesignerItem)item).BookmarkBase.GroupName),
                                                  new XElement("Position", string.Format("{0},{1},{2},{3}", Canvas.GetLeft(item), Canvas.GetTop(item), item.Width, item.Height)),
                                                  new XElement("ID", item.ID),
                                                  new XElement("zIndex", Canvas.GetZIndex(item)),
                                                  new XElement("IsGroup", item.IsGroup),
                                                  new XElement("ParentID", item.ParentID),
                                                  new XElement("Details", JsonHelper.ObjectToJson(item.BookmarkBase))
                                           //new XElement("Content", contentXaml)
                                              )
                                   );

            return serializedItems;
        }


        private string SerializeDesignerShape(IEnumerable<DesignerItem> designerItems)
        {
            List<string> template = new List<string>();
            StringBuilder sb = new StringBuilder();
            foreach (var item in designerItems)
            {
                var content = XamlWriter.Save(((DesignerItem)item).Content);
                if (!template.Contains(content))
                    template.Add(XamlWriter.Save(content));
            }
            foreach (var item in template)
            {
                sb.Append(item);
            }
            return sb.ToString();
        }


        private XElement SerializeConnections(IEnumerable<Connection> connections)
        {
            var serializedConnections = new XElement("Connections",
                           from connection in connections
                           select new XElement("Connection",
                                      new XElement("SourceID", connection.Source.ParentDesignerItem.ID),
                                      new XElement("SinkID", connection.Sink.ParentDesignerItem.ID),
                                      new XElement("SourceConnectorName", connection.Source.Name),
                                      new XElement("SinkConnectorName", connection.Sink.Name),
                                      new XElement("SourceArrowSymbol", connection.SourceArrowSymbol),
                                      new XElement("SinkArrowSymbol", connection.SinkArrowSymbol),
                                      new XElement("zIndex", Canvas.GetZIndex(connection)),
                                      new XElement("Condition", connection.Condition.Condition)
                                     )
                                  );

            return serializedConnections;
        }


        private static DesignerItem DeserializeDesignerItem(XElement itemXML, Guid id, double OffsetX, double OffsetY)
        {
            DesignerItem item = new DesignerItem(id);
            var poistion = itemXML.Element("Position").Value;
            double left = Double.Parse(poistion.Split(',')[0]);
            double top = Double.Parse(poistion.Split(',')[1]);
            double width = Double.Parse(poistion.Split(',')[2]);
            double height = Double.Parse(poistion.Split(',')[3]);
            string bType = itemXML.Element("BType").Value;
            string displayName = itemXML.Element("Name").Value;
            item.Width = width;
            item.Height = height;

            // 获取类型
            BookmarkBase bookMarkBask = BookmarkFactory.GetBookmark(EnumHelper.GetEnumByString<ActivityTypeEnum>(bType));
            item.BookmarkBase = (BookmarkBase)JsonHelper.JsonToObject(itemXML.Element("Details").Value.ToString(), bookMarkBask);

            item.DisplayName = displayName;
            item.ParentID = new Guid(itemXML.Element("ParentID").Value);
            item.IsGroup = Boolean.Parse(itemXML.Element("IsGroup").Value);
            Canvas.SetLeft(item, left + OffsetX);
            Canvas.SetTop(item, top + OffsetY);
            Canvas.SetZIndex(item, Int32.Parse(itemXML.Element("zIndex").Value));

            string xmlElement = XElement.Parse(item.BookmarkBase.GetSerialContent()).Value;
            Object obj = XamlReader.Load(XmlReader.Create(new StringReader(xmlElement)));
            item.Content = obj;

            //Object content = XamlReader.Load(XmlReader.Create(new StringReader(item.BookmarkBase.GetSerialContent())));
            //item.Content = content;

            //StringReader strreader = new StringReader(item.BookmarkBase.GetSerialContent());
            //XmlTextReader xmlreader = new XmlTextReader(strreader);
            //object obj = XamlReader.Load(xmlreader);

            //item.Content = obj;
            return item;
        }

        private void CopyCurrentSelection()
        {
            IEnumerable<DesignerItem> selectedDesignerItems =
                this.SelectionService.CurrentSelection.OfType<DesignerItem>();

            List<Connection> selectedConnections =
                this.SelectionService.CurrentSelection.OfType<Connection>().ToList();

            foreach (Connection connection in this.Children.OfType<Connection>())
            {
                if (!selectedConnections.Contains(connection))
                {
                    DesignerItem sourceItem = (from item in selectedDesignerItems
                                               where item.ID == connection.Source.ParentDesignerItem.ID
                                               select item).FirstOrDefault();

                    DesignerItem sinkItem = (from item in selectedDesignerItems
                                             where item.ID == connection.Sink.ParentDesignerItem.ID
                                             select item).FirstOrDefault();

                    if (sourceItem != null &&
                        sinkItem != null &&
                        BelongToSameGroup(sourceItem, sinkItem))
                    {
                        selectedConnections.Add(connection);
                    }
                }
            }

            XElement designerItemsXML = SerializeDesignerItems(selectedDesignerItems);
            XElement connectionsXML = SerializeConnections(selectedConnections);

            XElement root = new XElement("Root");
            root.Add(designerItemsXML);
            root.Add(connectionsXML);

            root.Add(new XAttribute("OffsetX", 10));
            root.Add(new XAttribute("OffsetY", 10));

            Clipboard.Clear();
            Clipboard.SetData(DataFormats.Xaml, root);
        }

        private void DeleteCurrentSelection()
        {
            foreach (Connection connection in SelectionService.CurrentSelection.OfType<Connection>())
            {
                this.Children.Remove(connection);
            }

            foreach (DesignerItem item in SelectionService.CurrentSelection.OfType<DesignerItem>())
            {
                Control cd = item.Template.FindName("PART_ConnectorDecorator", item) as Control;

                List<Connector> connectors = new List<Connector>();
                GetConnectors(cd, connectors);

                foreach (Connector connector in connectors)
                {
                    foreach (Connection con in connector.Connections)
                    {
                        this.Children.Remove(con);
                    }
                }
                this.Children.Remove(item);
            }

            SelectionService.ClearSelection();
            UpdateZIndex();
        }

        private void UpdateZIndex()
        {
            List<UIElement> ordered = (from UIElement item in this.Children
                                       orderby Canvas.GetZIndex(item as UIElement)
                                       select item as UIElement).ToList();

            for (int i = 0; i < ordered.Count; i++)
            {
                Canvas.SetZIndex(ordered[i], i);
            }
        }

        private static Rect GetBoundingRectangle(IEnumerable<DesignerItem> items)
        {
            double x1 = Double.MaxValue;
            double y1 = Double.MaxValue;
            double x2 = Double.MinValue;
            double y2 = Double.MinValue;

            foreach (DesignerItem item in items)
            {
                x1 = Math.Min(Canvas.GetLeft(item), x1);
                y1 = Math.Min(Canvas.GetTop(item), y1);

                x2 = Math.Max(Canvas.GetLeft(item) + item.Width, x2);
                y2 = Math.Max(Canvas.GetTop(item) + item.Height, y2);
            }

            return new Rect(new Point(x1, y1), new Point(x2, y2));
        }

        private void GetConnectors(DependencyObject parent, List<Connector> connectors)
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is Connector)
                {
                    connectors.Add(child as Connector);
                }
                else
                    GetConnectors(child, connectors);
            }
        }

        private Connector GetConnector(Guid itemID, String connectorName)
        {
            DesignerItem designerItem = (from item in this.Children.OfType<DesignerItem>()
                                         where item.ID == itemID
                                         select item).FirstOrDefault();

            Control connectorDecorator = designerItem.Template.FindName("PART_ConnectorDecorator", designerItem) as Control;
            connectorDecorator.ApplyTemplate();

            return connectorDecorator.Template.FindName(connectorName, connectorDecorator) as Connector;
        }

        private bool BelongToSameGroup(IGroupable item1, IGroupable item2)
        {
            IGroupable root1 = SelectionService.GetGroupRoot(item1);
            IGroupable root2 = SelectionService.GetGroupRoot(item2);

            return (root1.ID == root2.ID);
        }

        private void NameChanged_Executed(object sender, TextChangedEventArgs e)
        {
            MessageBox.Show("改了");
        }

        #endregion

        #region 保存模板导数据

        private void SaveTemplateToDataBase_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IEnumerable<DesignerItem> designerItems = this.Children.OfType<DesignerItem>();
            IEnumerable<Connection> connections = this.Children.OfType<Connection>();

            string designerItemShape = SerializeDesignerShape(designerItems);
            XElement designerItemsXML = SerializeDesignerItems(designerItems);
            XElement connectionsXML = SerializeConnections(connections);

            XElement root = new XElement("Root");
            root.Add(designerItemsXML);
            root.Add(connectionsXML);
            new NewTemplate(root.ToString()).Show();
        }


        private void OpenFromDataBase_Execute(object sender, ExecutedRoutedEventArgs e)
        {

            var templateManager = new TemplateInfoManager();
            templateManager.Show();
            templateManager.Closed += (s, r) =>
            {
                try
                {
                    if (templateManager.SelectTemplateInfo != null)
                    {
                        XElement root = XElement.Parse(templateManager.SelectTemplateInfo.TemplateText);
                        InitTemplate(root);
                    }
                }
                catch (Exception)
                {
                    TextBlock txtMsg = new TextBlock();
                    txtMsg.Text = "模板错误！";
                    txtMsg.Foreground = new SolidColorBrush(Colors.Red);
                    this.Children.Clear();
                    this.Children.Add(txtMsg);
                }
            };
        }


        private void InitTemplateBase()
        {
            XElement root = XElement.Load("TemplateBase.xml");
            InitTemplate(root);
        }


        private void InitTemplate(XElement root)
        {
            if (root == null)
                return;

            TemplateHistory.SingObj.Add(DateTime.Now.ToString(), root);
            this.Children.Clear();
            this.SelectionService.ClearSelection();

            IEnumerable<XElement> itemsXML = root.Elements("DesignerItems").Elements("DesignerItem");
            foreach (XElement itemXML in itemsXML)
            {
                Guid id = new Guid(itemXML.Element("ID").Value);

                DesignerItem item = DeserializeDesignerItem(itemXML, id, 0, 0);
                item.OthersVisibility = Visibility.Collapsed;
                if (XmlHelper.GetSafeValue(itemXML, ActivityConst.TYPE) == ActivityTypeEnum.Process.ToString())
                {
                    var bookmark = JsonHelper.JsonToT<StandBookmark>(XmlHelper.GetSafeValue(itemXML, ActivityConst.DETAILS));
                    if (bookmark.IsParallel)
                        item.OthersVisibility = Visibility.Visible;
                }
                this.Children.Add(item);
                SetConnectorDecoratorTemplate(item);
            }

            this.InvalidateVisual();

            IEnumerable<XElement> connectionsXML = root.Elements("Connections").Elements("Connection");
            foreach (XElement connectionXML in connectionsXML)
            {
                Guid sourceID = new Guid(connectionXML.Element("SourceID").Value);
                Guid sinkID = new Guid(connectionXML.Element("SinkID").Value);

                String sourceConnectorName = connectionXML.Element("SourceConnectorName").Value;
                String sinkConnectorName = connectionXML.Element("SinkConnectorName").Value;

                Connector sourceConnector = GetConnector(sourceID, sourceConnectorName);
                Connector sinkConnector = GetConnector(sinkID, sinkConnectorName);

                Connection connection = new Connection(sourceConnector, sinkConnector);
                connection.Condition.Condition = connectionXML.Element("Condition").Value;
                Canvas.SetZIndex(connection, Int32.Parse(connectionXML.Element("zIndex").Value));
                this.Children.Add(connection);
            }
        }

        #endregion (保存模板导数据)

        #region 模板类型管理


        private void AddTemplateType_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // 弹出窗口管理
            new TemplateTypeManager().Show();
        }


        private void ManagerTemplateType_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            new TemplateInfoManager().ShowDialog();
        }

        #endregion (模板类型管理)

        #region 模板调试

        private void DebugTemplate_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            new DebugWorkflowInfo().Show();
        }

        #endregion (模板调试)

        #region 详细属性信息

        private void Details_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectItems = this.SelectionService.CurrentSelection.OfType<DesignerItem>();
            if (selectItems.Count() == 1)
            {
                DesignerCanvas t = sender as DesignerCanvas;
                if (t.AssociateProperty != null)
                {
                    // 可以设置属性
                    t.AssociateProperty.Instance = selectItems.First().BookmarkBase;
                }
            }
        }

        #endregion Detail Command

        #region 设置分支条件

        private void SettingCondition_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            var connection = SelectionService.CurrentSelection[0] as Connection;
            DesignerCanvas t = sender as DesignerCanvas;
            t.AssociateProperty.Instance = connection.Condition;
        }

        #endregion (设置分支条件)

        #region 检验模板正确

        private void Validate_Excecute(object sender, ExecutedRoutedEventArgs e)
        {

            

            IEnumerable<DesignerItem> designerItems = this.Children.OfType<DesignerItem>();
            IEnumerable<Connection> connections = this.Children.OfType<Connection>();

            string designerItemShape = SerializeDesignerShape(designerItems);
            XElement designerItemsXML = SerializeDesignerItems(designerItems);
            XElement connectionsXML = SerializeConnections(connections);

            XElement root = new XElement("Root");
            root.Add(designerItemsXML);
            root.Add(connectionsXML);
            List<string> lst = new List<string>();
            Dictionary<string, Stack<string>> exeResults=null;
            
            var routingRule = new RoutingRule(root);
            //routingRule.TransGraph();

            (e.Parameter as Window1).txbError.Text = routingRule.TemplateValidate();
            
            
            //return;
            //(e.Parameter as Window1).txbError.Text = new Nsun.Workflow.Core.Validation.DesignRuleValidate().ValidateMsg(root, lst,ref exeResults);


            
            new System.Threading.Thread(() =>
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    int i = 0;
                    foreach (var item in routingRule.Routing())
                    {
                        i++;
                        foreach (var designerItem in designerItems)
                        {
                            if (designerItem.DisplayName == item)
                            {
                                //designerItem.IsRunning = "Red";
                                designerItem.IsDebuging = Visibility.Visible;
                                designerItem.RunState = designerItem.RunState ?? "" + i.ToString();
                                //if (exeResults.ContainsKey(ExeResultConst.NoParallelEnds) && exeResults[ExeResultConst.NoParallelEnds].Contains(item))
                                //{
                                //    designerItem.RunState += designerItem.RunState + "_Error";
                                //}
                            }
                        }
                    }
                }));
            }).Start();
        }

        #endregion (检验模板正确)

        #region 历史模板

        private void CanHisTemplate_Execute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = TemplateHistory.SingObj.CanBack;
        }

        private void HisTemplate_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            var temp = TemplateHistory.SingObj.Back().Value;
            if (temp != null)
                InitTemplate(TemplateHistory.SingObj.Back().Value);
        }

        #endregion(历史模板)


        private void GetTemplateInfo_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            var selectItems = this.SelectionService.CurrentSelection.OfType<DesignerItem>();
            if (selectItems.Count() == 1)
            {
                DesignerCanvas t = sender as DesignerCanvas;
                if (t.AssociateProperty != null)
                {
                    // 可以设置属性
                    if (selectItems.First().BookmarkBase is SubRoutingsBookmark)
                    {
                        var sub = selectItems.First().BookmarkBase as SubRoutingsBookmark;
                        if (selectItems.First().BookmarkBase is SubRoutingsBookmark)
                        {
                            var subs = selectItems.First().BookmarkBase as SubRoutingsBookmark;
                            TemplateInfoManager tm = new TemplateInfoManager();
                            tm.Closed += (s, r) =>
                            {
                                if (tm.DialogResultExt == true)
                                {
                                    sub.TemplateName = tm.SelectTemplateInfo.TemplateName;
                                    sub.TemplateId = tm.SelectTemplateInfo.Id;
                                }
                            };
                            tm.ShowDialog();
                        }
                    }
                    else if (selectItems.First().BookmarkBase is SubRoutingBookmark)
                    {
                        var sub = selectItems.First().BookmarkBase as SubRoutingBookmark;
                        if (selectItems.First().BookmarkBase is SubRoutingBookmark)
                        {
                            var subs = selectItems.First().BookmarkBase as SubRoutingBookmark;
                            TemplateInfoManager tm = new TemplateInfoManager();
                            tm.Closed += (s, r) =>
                            {
                                if (tm.DialogResultExt == true)
                                {
                                    sub.TemplateName = tm.SelectTemplateInfo.TemplateName;
                                    sub.TemplateId = tm.SelectTemplateInfo.Id;
                                }
                            };
                            tm.ShowDialog();
                        }
                    }
                }
            }
        }

        private void CanGetTemplateInfo_Execute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            var selectItems = this.SelectionService.CurrentSelection.OfType<DesignerItem>();
            if (selectItems.Count() == 1)
            {
                DesignerCanvas t = sender as DesignerCanvas;
                if (t.AssociateProperty != null)
                {
                    // 可以设置属性
                    if (selectItems.First().BookmarkBase is SubRoutingBookmark || selectItems.First().BookmarkBase is SubRoutingsBookmark)
                        e.CanExecute = true;
                }
            }
        }

        private void SettingParallNode_Excute(object sender, ExecutedRoutedEventArgs e)
        {
            var selectItems = this.SelectionService.CurrentSelection.OfType<DesignerItem>();
            if (selectItems.Count() == 1)
            {
                DesignerCanvas t = sender as DesignerCanvas;
                if (t.AssociateProperty != null)
                {
                    // 可以设置属性

                    if (selectItems.First().BookmarkBase is StandBookmark)
                    {
                        var bookmark = selectItems.First().BookmarkBase as StandBookmark;
                        if (bookmark.IsParallel == true)
                        {
                            bookmark.IsParallel = false;
                            selectItems.First().OthersVisibility = Visibility.Collapsed;
                        }
                        else
                        {
                            bookmark.IsParallel = true;
                            selectItems.First().OthersVisibility = Visibility.Visible;
                        }
                    }
                }
            }
        }

        private void CanSettingParallelNode_Exectue(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            var selectItems = this.SelectionService.CurrentSelection.OfType<DesignerItem>();
            if (selectItems.Count() == 1)
            {
                DesignerCanvas t = sender as DesignerCanvas;
                if (t.AssociateProperty != null)
                {
                    // 可以设置属性
                    if (selectItems.First().BookmarkBase is StandBookmark)
                        e.CanExecute = true;
                }
            }
        }


        private void LinkChildTemplate_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            var selectItems = this.SelectionService.CurrentSelection.OfType<DesignerItem>();
            if (selectItems.Count() == 1)
            {
                DesignerCanvas t = sender as DesignerCanvas;
                if (t.AssociateProperty != null)
                {
                    Guid templateId = Guid.Empty;
                    // 可以设置属性
                    if (selectItems.First().BookmarkBase is SubRoutingsBookmark)
                    {
                        var sub = selectItems.First().BookmarkBase as SubRoutingsBookmark;
                        templateId = sub.TemplateId;

                    }
                    else if (selectItems.First().BookmarkBase is SubRoutingBookmark)
                    {
                        var sub = selectItems.First().BookmarkBase as SubRoutingBookmark;
                        templateId = sub.TemplateId;

                    }

                    if (templateId != Guid.Empty)
                    {
                        // 从服务中获取模板
                        var template = new Service1Client().GetTemplateByIds(templateId);
                        if (template != null)
                        {
                            string templateXml = template.TemplateText;
                            var doc = XElement.Parse(templateXml);
                            InitTemplate(doc);
                        }
                    }
                }
            }
        }
    }
}
