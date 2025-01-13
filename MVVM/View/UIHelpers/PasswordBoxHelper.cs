using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace ProjectTracker.MVVM.View.UIHelpers
{
    public static class PasswordBoxHelper
    {
        public static readonly DependencyProperty SecurePasswordProperty =
            DependencyProperty.RegisterAttached("SecurePassword", typeof(SecureString), 
                typeof(PasswordBoxHelper), new PropertyMetadata(default(SecureString), OnSecurePasswordChanged));
        
        public static SecureString GetSecurePassword(DependencyObject d)
        {
            return (SecureString)d.GetValue(SecurePasswordProperty);
        }
        
        public static void SetSecurePassword(DependencyObject d, SecureString value)
        {
            d.SetValue(SecurePasswordProperty, value);
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox == null) return;

            SetSecurePassword(passwordBox, passwordBox.SecurePassword.Copy());
        }

        private static void OnSecurePasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var passwoedBox = d as PasswordBox;
            if (passwoedBox == null) return;

            passwoedBox.PasswordChanged -= PasswordBox_PasswordChanged;

            if(e.NewValue != null)
            {
                passwoedBox.PasswordChanged += PasswordBox_PasswordChanged;
            }
        }        
    }
}
