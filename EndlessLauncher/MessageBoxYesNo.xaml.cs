using System.Windows;
using System.Windows.Input;

namespace EndlessLauncher
{
    /// <summary>
    /// Interaction logic for CloseMessageBox.xaml
    /// </summary>
    public partial class MessageBoxYesNo : Window
    {
        public MessageBoxYesNo(string Message, string YesText, string NoText)
        {
            InitializeComponent();

            InfoText.Text = Message;
            Accept.Content = YesText;
            Cancel.Content = NoText;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Minimum_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            
        }
    }
}
