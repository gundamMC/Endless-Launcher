using System.Windows;
using System.Windows.Input;

namespace EndlessLauncher
{
    /// <summary>
    /// Interaction logic for MessageBoxOK.xaml
    /// </summary>
    public partial class MessageBoxOK : Window
    {
        public MessageBoxOK(string Message, string OkText)
        {
            InitializeComponent();

            InfoText.Text = Message;
            Ok.Content = OkText;

            if (OkText.Length > 50) // "dyanmic" font size
                Ok.FontSize = 18;

            if (OkText.Length > 100)
                Ok.FontSize = 15;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Minimum_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
