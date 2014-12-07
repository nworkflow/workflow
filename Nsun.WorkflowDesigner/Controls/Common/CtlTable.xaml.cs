using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Nsun.Common.UC
{
    /// <summary>
    /// CtlTable.xaml 的互動邏輯
    /// </summary>
    public partial class CtlTable : UserControl, INotifyPropertyChanged
    {
        public CtlTable()
        {
            InitializeComponent();
            this.DataContext = this;
        }


        public static readonly DependencyProperty LabelNameProperty = DependencyProperty.Register("LabelName", typeof(string), typeof(CtlTable));
        public static readonly DependencyProperty LabelContentProperty = DependencyProperty.Register("LabelContent", typeof(FrameworkElement), typeof(CtlTable));
        public static readonly DependencyProperty LabelColumn0_WidthProperty = DependencyProperty.Register("LabelColumn0_Width", typeof(double), typeof(CtlTable));
        public static readonly DependencyProperty LabelBorderThicknessProperty = DependencyProperty.Register("LabelBorderThickness", typeof(Thickness), typeof(CtlTable));

        public string LabelName
        {
            get
            {
                return (string)GetValue(LabelNameProperty);
            }
            set
            {
                SetValue(LabelNameProperty, value);
                RaisePropertyChanged("LabelName");
            }
        }


        public FrameworkElement LabelContent
        {
            get { return (FrameworkElement)GetValue(LabelContentProperty); }
            set
            {
                SetValue(LabelContentProperty, value);
                RaisePropertyChanged("LabelContent");
            }
        }


        public double LabelColumn0_Width
        {
            get { return (double)GetValue(LabelColumn0_WidthProperty); }
            set
            {
                SetValue(LabelColumn0_WidthProperty, value);
                RaisePropertyChanged("LabelColumn0_Width");
            }
        }

        public Thickness LabelBorderThickness
        {
            get { return (Thickness)GetValue(LabelBorderThicknessProperty); }
            set
            {
                SetValue(LabelBorderThicknessProperty, value);
                RaisePropertyChanged("LabelBorderThickness");
            }
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
