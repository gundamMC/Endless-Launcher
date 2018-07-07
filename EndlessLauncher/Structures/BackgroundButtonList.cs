using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EndlessLauncher
{
    public class BackgroundButtonClass
    {
        public BackgroundButtonClass(Image inputIcon, MainWindow inputForm, String Size, int inputIndex)
        {
            Icon = inputIcon;
            Form = inputForm;
            Index = inputIndex;

            if (Size.Equals("small"))
            {
                Width = 150;
                BackgroundOffset = 55;
                LabelOffset = 35;
            }
            else if (Size.Equals("medium"))
            {
                Width = 310;
                BackgroundOffset = 135;
                LabelOffset = 95;
            }

            #region initialize Controls

            ContextMenu IconContextMenu = new ContextMenu();

            IconContextMenu.Items.Add("Resize (Large)");
            MenuItem HideItem = new MenuItem() { Header = "Hide" };
            HideItem.Click += ContextMenuHide;
            IconContextMenu.Items.Add(HideItem);
            IconContextMenu.Name = Icon.Name + "ContextMenu";
            Form.RegisterName(Icon.Name + "ContextMenu", IconContextMenu);

            Button IconBackground = new Button()
            {
                Name = Icon.Name + "Background",
                Height = 150,
                Width = Width,
                BorderThickness = new Thickness(0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                //Effect = new System.Windows.Media.Effects.BlurEffect() { Radius = 20, KernelType = System.Windows.Media.Effects.KernelType.Gaussian },
                //This seems to only apply the blur to itself, excluding the background image
                Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/MagnetBackground.png"))),
                BorderBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/MagnetBackground_Hover.png"))),

                ContextMenu = IconContextMenu
            };

            Icon.ContextMenu = IconContextMenu;

            IconBackground.PreviewMouseLeftButtonDown += Form.Icon_MouseLeftButtonDown;
            IconBackground.PreviewMouseLeftButtonUp += Form.Icon_MouseLeftButtonUp;
            IconBackground.PreviewMouseMove += Form.Icon_MouseMove;

            Form.RegisterName(Icon.Name + "Background", IconBackground);

            Panel.SetZIndex(IconBackground, -1);

            BackgroundButton = IconBackground;

            //End of button background


            //Starting button label

            string LabelName = "";
            foreach (LanguageClass.LabelContentClass ii in App.Lang.LabelContents)
            {
                if (Icon.Name == ii.IconName)
                    LabelName = ii.LabelText;
            }

            Label IconLabel = new Label()
            {
                Name = Icon.Name + "Label",
                BorderThickness = new Thickness(0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Content = LabelName,
                FontSize = 12,
                FontFamily = new FontFamily("Microsoft Yahei UI"),
                FontWeight = FontWeights.Light,
                Foreground = new SolidColorBrush(Colors.White),
                HorizontalContentAlignment = HorizontalAlignment.Left
            };

            IconLabel.PreviewMouseLeftButtonDown += Form.Icon_MouseLeftButtonDown;
            IconLabel.PreviewMouseLeftButtonUp += Form.Icon_MouseLeftButtonUp;
            IconLabel.PreviewMouseMove += Form.Icon_MouseMove;

            Form.RegisterName(Icon.Name + "Label", IconLabel);

            TextLabel = IconLabel;

            #endregion

            InitPos();

            

            Form.UpdateLayout();
        }

        private MainWindow Form { get; set; }

        public Image Icon { get; set; }

        public Button BackgroundButton { get; set; }

        public Label TextLabel { get; set; }

        public ContextMenu ContextMenu { get; set; }

        public int Width { get; set; }

        public int BackgroundOffset { get; set; }

        public int LabelOffset { get; set; }

        public Point Location { get; set; }

        public Point ExtendedLocation { get; set; }

        public int Index { get; set; }


        public void UpdateOffset()
        {
            BackgroundButton.Margin = new Thickness(Location.X - BackgroundOffset, Location.Y - 55, 0, 0);
            TextLabel.Margin = new Thickness(Location.X - LabelOffset, Location.Y + 50, 0, 0);
        }

        private void ContextMenuHide(object sender, RoutedEventArgs e)
        {
            Icon.Visibility = Visibility.Hidden;
            BackgroundButton.Visibility = Visibility.Hidden;
            TextLabel.Visibility = Visibility.Hidden;

             // adds point back to the list
        }

        private void InitPos()
        {

            Icon.Margin = new Thickness(IconPointClass.IconPoints[Index].X, IconPointClass.IconPoints[Index].Y, 0, 0);
            if (Width == 310)
                Icon.Margin = new Thickness(Icon.Margin.Left + 64, Icon.Margin.Top, 0, 0);

            UpdateOffset();

            Form.MainGrid.Children.Add(BackgroundButton);
            Form.MainGrid.Children.Add(TextLabel);

            Location = new Point(Icon.Margin.Left, Icon.Margin.Top);
        }

    }


}
