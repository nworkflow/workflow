using System.Windows;
using System.Windows.Input;

namespace Resource_Dictionaries
{
    public partial class BaseStyleWindow : ResourceDictionary
    {
        public BaseStyleWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the MouseLeftButtonDown event. This event handler is used here to facilitate
        /// dragging of the Window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void titleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;
            window.DragMove();
        }

        /// <summary>
        /// Fires when the user clicks the Close button on the window's custom title bar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;
            window.Close();
        }

        /// <summary>
        /// Fires when the user clicks the minimize button on the window's custom title bar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void minimizeButton_Click(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;
            window.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Fires when the user clicks the maximize button on the window's custom title bar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void maximizeButton_Click(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;
            // Check the current state of the window. If the window is currently maximized, return the
            // window to it's normal state when the maximize button is clicked, otherwise maximize the window.
            if (window.WindowState == WindowState.Maximized)
                window.WindowState = WindowState.Normal;
            else window.WindowState = WindowState.Maximized;
        }
    }
}