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
        public static RegistryConfig Config;


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists("EnlessConfig.json"))
            {
                Config = JsonMapper.ToObject<RegistryConfig>(System.IO.File.ReadAllText("EnlessConfig.json"));
            }
            else { }
             
        }

        private void HeaderBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
