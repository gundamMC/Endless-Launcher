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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void HeaderBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(UsernameInputBox.Text) || String.IsNullOrWhiteSpace(PasswordInputBox.Password))
            {
                MessageBoxOK form = new MessageBoxOK("You must input your username and password", "OK");
                form.ShowDialog();
            }

            //tries to authenicate using username and password
        }
    }
}
