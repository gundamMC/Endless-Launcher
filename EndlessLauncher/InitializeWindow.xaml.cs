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
    /// Interaction logic for InitializeWindow.xaml
    /// </summary>
    public partial class InitializeWindow : Window
    {
        public InitializeWindow()
        {
            InitializeComponent();
        }

        private void HeaderBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            GetMojangLauncherInfo.MojangAuthInfo AuthInfo = GetMojangLauncherInfo.GetMojangAuthInfo();


            if (!String.IsNullOrWhiteSpace(AuthInfo.Displayname))
            {
                //Returned info from mojang launcher correctly
                //Refresh token
            }
            else
            {
                //show login page
                LoginWindow newWindow = new LoginWindow();
                newWindow.Show();
                this.Hide();
            }

            
        }
    }
}
