using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DiagramDesigner
{
    // Implements ItemsControl for ToolboxItems    
    public class Toolbox : ItemsControl
    {
        // Defines the ItemHeight and ItemWidth properties of
        // the WrapPanel used for this Toolbox
        public Size ItemSize
        {
            get { return itemSize; }
            set { itemSize = value; }
        }

        private Size itemSize = new Size(200, 24);

        // Creates or identifies the element that is used to display the given item.        
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ToolboxItem();
        }

        // Determines if the specified item is (or is eligible to be) its own container.        
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is ToolboxItem);
        }


        public void Name_TextChanged(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("成功了");
        }
    }
}
