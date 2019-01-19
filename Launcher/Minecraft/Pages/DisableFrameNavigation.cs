using System.Windows;
using System.Windows.Navigation;

namespace Frame
{
    public static class FrameNavigation
    {
        public static bool GetDisable(DependencyObject o)
        {
            return (bool)o.GetValue(DisableProperty);
        }
        public static void SetDisable(DependencyObject o, bool value)
        {
            o.SetValue(DisableProperty, value);
        }

        public static readonly DependencyProperty DisableProperty =
            DependencyProperty.RegisterAttached("Disable", typeof(bool), typeof(FrameNavigation),
                                                new PropertyMetadata(false, DisableChanged));

        public static void DisableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var frame = (System.Windows.Controls.Frame)sender;
            frame.Navigated += DontNavigate;
            frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }

        public static void DontNavigate(object sender, NavigationEventArgs e)
        {
            ((System.Windows.Controls.Frame)sender).NavigationService.RemoveBackEntry();
        }
    }
}
