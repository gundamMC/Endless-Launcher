using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
            DragMove();
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

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseB_Click(object sender, RoutedEventArgs e)
        {
            //Pressed texture
            CloseB.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Close_pressed.png")));

            MessageBoxYesNo Form = new MessageBoxYesNo("Close Endless Launcher?", "Close", "Cancel");
            if (Form.ShowDialog() == true)
            {
                Application.Current.Shutdown();
            }
            else
            {
                CloseB.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Close.png")));
            }
        }
    }
}
