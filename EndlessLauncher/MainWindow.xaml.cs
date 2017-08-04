using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EndlessLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public static ConfigClass Config;


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists("EnlessConfig.json"))
            {
                Config = JsonMapper.ToObject<ConfigClass>(System.IO.File.ReadAllText("EnlessConfig.json"));
            }
            else { }
             
        }

        private void HeaderBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CloseButton.Background = new ImageBrush(new BitmapImage(new Uri(@"Resources/Close_pressed.png")));
            CloseMessageBox Form = new CloseMessageBox();
            Form.Show();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

    }
}
