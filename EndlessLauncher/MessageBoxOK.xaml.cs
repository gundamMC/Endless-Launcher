using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
