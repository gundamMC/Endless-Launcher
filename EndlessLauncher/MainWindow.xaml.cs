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

        private void LoadMagnetBackgrounds(Image i)
        {
            Button MagnetBackgroundMedium = new Button()
            {
                Height = 150,
                Width = 150,
                BorderThickness = new Thickness(0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                //Effect = new System.Windows.Media.Effects.BlurEffect() { Radius = 20, KernelType = System.Windows.Media.Effects.KernelType.Gaussian },
                //This seems to only apply the blur to itself, excluding the background image
                Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/MagnetBackground.png")))
            };

            Panel.SetZIndex(MagnetBackgroundMedium, -1);

            //Calculates the X and Y cords based on the icons
            Point relativePoint = i.TransformToAncestor(this).Transform(new Point(0, 0));
            MagnetBackgroundMedium.Margin = new Thickness(relativePoint.X - 55, relativePoint.Y - 55, 0, 0);

            MainGrid.Children.Add(MagnetBackgroundMedium);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists("EnlessConfig.json"))
            {
                Config = JsonMapper.ToObject<ConfigClass>(System.IO.File.ReadAllText("EnlessConfig.json"));
            }
            else { }


            foreach (Image i in LeftGird.Children)
            {
                LoadMagnetBackgrounds(i);
            }

            foreach (Image i in RightGrid.Children)
            {
                LoadMagnetBackgrounds(i);
            }
        }

        private void HeaderBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            //Pressed texture
            CloseButton.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Close_pressed.png")));

            CloseMessageBox Form = new CloseMessageBox();
            Form.ShowDialog();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

    }
}
