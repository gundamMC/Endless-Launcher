using LitJson;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            MagnetBackgroundMedium.PreviewMouseLeftButtonDown += Icon_MouseLeftButtonDown;
            MagnetBackgroundMedium.PreviewMouseLeftButtonUp += Icon_MouseLeftButtonUp;
            MagnetBackgroundMedium.PreviewMouseMove += Icon_MouseMove;

            this.RegisterName(i.Name + "Button", MagnetBackgroundMedium);

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
        private WrapPanel OriginalParent;
        int MoveRefreshLock = 0;


        private void Icon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsDragging = true;

            if (sender is Button)
            {
                string Sendername = ((Button)sender).Name;
                sender = (Image)this.FindName(Sendername.Replace("Button", ""));
            }

            var DraggedObject = sender as Image;

            Point OriginalPos = (DraggedObject).TransformToAncestor(this).Transform(new Point(0, 0));

            OriginalParent = DraggedObject.Parent as WrapPanel;
            OriginalParent.Children.Remove(DraggedObject);
            MainGrid.Children.Add(DraggedObject);

            DraggedObject.Margin = new Thickness(OriginalPos.X, OriginalPos.Y, 0, 0);

        }



        private void Icon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsDragging = false;
            MoveRefreshLock = 0;


            if (sender is Button)
            {
                string Sendername = ((Button)sender).Name;
                sender = (Image)this.FindName(Sendername.Replace("Button", ""));
            }

            var DraggedObject = sender as Image;

            Boolean Added = false;

            if (DraggedObject.Margin.Top > 215 && 515 > DraggedObject.Margin.Top)
            {
                if (DraggedObject.Margin.Left > 150 && 410 > DraggedObject.Margin.Left)
                {
                    //Add to Left Grid

                    MainGrid.Children.Remove(DraggedObject);
                    //LeftGird.Children.Add(DraggedObject);
                    LeftGird.Children.Insert(LeftGird.Children.Count, DraggedObject);

                    DraggedObject.Margin = new Thickness(45, 55, 45, 55);

                    Added = true;
                }

                if (DraggedObject.Margin.Left > 450 && 860 > DraggedObject.Margin.Left)
                {
                    //Add to Right Grid

                    MainGrid.Children.Remove(DraggedObject);
                    //RightGrid.Children.Add(DraggedObject);
                    RightGrid.Children.Insert(RightGrid.Children.Count, DraggedObject);

                    DraggedObject.Margin = new Thickness(45, 55, 45, 55);

                    Added = true;
                }
            }

            if (Added == false)
            {
                //Resets back to original
                MainGrid.Children.Remove(DraggedObject);
                OriginalParent.Children.Add(DraggedObject);

                DraggedObject.Margin = new Thickness(45, 55, 45, 55);

            }
        }

        

        private void Icon_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDragging)
            {
                if (MoveRefreshLock <= 1)
                {
                    //Refreshes it twice so that the icons get rearranged before repositioning the background buttons
                    UpdateIconBackgrounds(OriginalParent);
                    MoveRefreshLock++;
                }

                if (sender is Button)
                {
                    string Sendername = ((Button)sender).Name;
                    sender = (Image)this.FindName(Sendername.Replace("Button", ""));
                }

                var DraggedObject = sender as Image;

                var DraggedBackground = (Button)this.FindName(DraggedObject.Name + "Button");

                //double MouseMoved = e.GetPosition().X;

                DraggedObject.Margin = new Thickness(e.GetPosition(this).X - 20, e.GetPosition(this).Y - 20, 0, 0);
                DraggedBackground.Margin = new Thickness(e.GetPosition(this).X - 75, e.GetPosition(this).Y - 75, 0, 0);
            }

        }

        private void UpdateIconBackgrounds(object sender)
        {
            foreach (Image i in ((WrapPanel)sender).Children)
            {
                Point relativePoint = i.TransformToAncestor(this).Transform(new Point(0, 0));

                Button DraggedBackground = (Button)this.FindName(i.Name + "Button");
                DraggedBackground.Margin = new Thickness(relativePoint.X - 55, relativePoint.Y - 55, 0, 0);
            }
        }
    }
}
