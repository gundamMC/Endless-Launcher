using LitJson;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

        public List<BackgroundButtonClass> Backgrounds = new List<BackgroundButtonClass>();

        //public List<Point> IconPoints = new List<Point>();

        private void LoadMagnetBackgrounds(Image i)
        {
            Button ButtonBackground = new Button()
            {
                Name = i.Name + "Button",
                Height = 150,
                Width = 150,
                BorderThickness = new Thickness(0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                //Effect = new System.Windows.Media.Effects.BlurEffect() { Radius = 20, KernelType = System.Windows.Media.Effects.KernelType.Gaussian },
                //This seems to only apply the blur to itself, excluding the background image
                Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/MagnetBackground.png"))),
                BorderBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/MagnetBackground_Hover.png")))
            };

            ButtonBackground.PreviewMouseLeftButtonDown += Icon_MouseLeftButtonDown;
            ButtonBackground.PreviewMouseLeftButtonUp += Icon_MouseLeftButtonUp;
            ButtonBackground.PreviewMouseMove += Icon_MouseMove;

            this.RegisterName(i.Name + "Button", ButtonBackground);

            Panel.SetZIndex(ButtonBackground, -1);

            //Calculates the X and Y cords based on the icons
            Point relativePoint = i.TransformToAncestor(this).Transform(new Point(0, 0));
            ButtonBackground.Margin = new Thickness(relativePoint.X - 55, relativePoint.Y - 55, 0, 0);

            MainGrid.Children.Add(ButtonBackground);

            Backgrounds.Add(new BackgroundButtonClass()
            {
                BackgroundButton = ButtonBackground,
                Icon = i,
                Size = 1
            });

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            

            if (File.Exists("EnlessConfig.json"))
            {
                Config = JsonMapper.ToObject<ConfigClass>(System.IO.File.ReadAllText("EnlessConfig.json"));
            }
            else { }

            foreach (Image i in ButtonsGrid.Children)
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
            CloseB.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Close_pressed.png")));

            CloseMessageBox Form = new CloseMessageBox();
            Form.ShowDialog();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Icon_MouseEnter(object sender, MouseEventArgs e)
        {
            string imagename = (sender as Image).Name;
            var targetbutton = (Button)this.FindName(imagename + "Button");
            targetbutton.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/MagnetBackground_Hover.png")));
        }

        private void Icon_MouseLeave(object sender, MouseEventArgs e)
        {
            string imagename = (sender as Image).Name;
            var targetbutton = (Button)this.FindName(imagename + "Button");
            targetbutton.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/MagnetBackground.png")));
        }

        private Boolean IsDragging = false;
        private Image DraggedObject;
        private Button DraggedBackground;
        private Thickness OriginalPos;


        private void Icon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsDragging = true;

            if (sender is Button)
            {
                DraggedBackground = sender as Button;
                string Sendername = ((Button)sender).Name;
                DraggedObject = (Image)this.FindName(Sendername.Replace("Button", ""));
            }
            else
            {
                DraggedObject = sender as Image;
                DraggedBackground = (Button)this.FindName(DraggedObject.Name + "Button");
            }

            OriginalPos = DraggedObject.Margin;
        }



        private void Icon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DraggedObject == null)
            {
                return;
            }

            IsDragging = false;


            DraggedObject.Margin = PointDistance.GetClosestPoint(DraggedObject.Margin, IconPointClass.IconPoints, OriginalPos, DraggedBackground);


            DraggedObject = null;

            //UpdateIconBackgrounds();
        }

        

        private void Icon_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDragging && DraggedObject != null)
            {

                DraggedObject.Margin = new Thickness(e.GetPosition(this).X - 20, e.GetPosition(this).Y - 20, 0, 0);
                DraggedBackground.Margin = new Thickness(e.GetPosition(this).X - 75, e.GetPosition(this).Y - 75, 0, 0);

            }

        }

        private void HeaderStartGame_Click(object sender, RoutedEventArgs e)
        {
            //Launch.LaunchGame("", 25565, Config.Version, "EN");
        }

        private void UpdateIconBackgrounds()
        {
            foreach (BackgroundButtonClass i in Backgrounds)
            {
                Point relativePoint = i.Icon.TransformToAncestor(this).Transform(new Point(0, 0));

                i.BackgroundButton.Margin = new Thickness(relativePoint.X - 55, relativePoint.Y - 55, 0, 0);
            }
        }
    }
}
