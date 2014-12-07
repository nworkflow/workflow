using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Nsun.Tools.WorkflowDesigner.Controls
{
    /// <summary>
    /// ToolBoxItemExt.xaml 的互動邏輯
    /// </summary>
    public partial class ToolBoxItemExt : UserControl, INotifyPropertyChanged
    {
        private string _toolTipShow;
        public string ToolTipShow
        {
            get { return _toolTipShow; }
            set
            {
                _toolTipShow = value;
                RaisePropertyChanged("ToolTipShow");
            }
        }

        private string _imageHeader;
        public string ImageHeader
        {
            get { return _imageHeader; }
            set
            {
                _imageHeader = value;
                RaisePropertyChanged("ImageHeader");
            }
        }

        private string _itemContext;
        public string ItemContext
        {
            get { return _itemContext; }
            set
            {
                _itemContext = value;
                RaisePropertyChanged("ItemContext");
            }
        }


        private string _bookmarkType;
        public string BookmarkType
        {
            get { return _bookmarkType; }
            set { _bookmarkType = value;
            RaisePropertyChanged("BookmarkType");
            }
        }

        public ToolBoxItemExt()
        {
        }

        public ToolBoxItemExt(string toolTip, string context, string image,string bookmarkType)
        {
            InitializeComponent();
            this.DataContext = this;
            this.ToolTipShow = toolTip;
            this.ItemContext = context;
            this.BookmarkType = bookmarkType;
            this.ImageHeader = string.IsNullOrEmpty(image) ? @"/Nsun.Tools.WorkflowDesigner;component/Resources/Images/address-book--pencil.png" : image;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
