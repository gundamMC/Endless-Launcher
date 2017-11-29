﻿using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

            if (!File.Exists("EnlessConfig.json"))
            {
                //App.Config = JsonMapper.ToObject<ConfigClass>(System.IO.File.ReadAllText("EnlessConfig.json"));

                if (App.Config.TextLanguage && File.Exists("EndlessLang.json"))

                    App.Lang = JsonMapper.ToObject<LanguageClass>(System.IO.File.ReadAllText("EnlessConfig.json"));

                else
                {   // load default english text
                    App.Lang.MainWindow.Title = "Endless Launcher";
                    App.Lang.MainWindow.Hello = "Hello, {0}";
                    App.Lang.MainWindow.ConnectionText.Connected = "Connected to Mojang servers";
                    App.Lang.MainWindow.ConnectionText.Failed = "Could not establish connection to Mojang servers";
                    App.Lang.MainWindow.ConnectionText.ConnectionLost = "Connection to Mojang servers was lost";
                    App.Lang.MainWindow.UUID = "UUID: {0}";
                    App.Lang.MainWindow.SelectedVersion = "Selected version";
                    App.Lang.MainWindow.StartGame = "Start Game";
                }
                    

                InitializeText();
            }
            else
            {
                InitializeWindow window = new InitializeWindow();
                window.Show();
                this.Hide();
            }

        }

        //
        // VARIABLES:
        //
        public List<BackgroundButtonClass> Backgrounds = new List<BackgroundButtonClass>();
        //

        public void InitializeText()
        {

            TitleLabel.Content = App.Lang.MainWindow.Title;

            HelloLabel.Content = App.Lang.MainWindow.Hello.Replace("{0}", App.Config.DisplayName);

            ConnectionLabel.Content = App.Lang.MainWindow.ConnectionText.Connected;    // add status check

            UUIDLabel.Content = App.Lang.MainWindow.UUID.Replace("{0}", App.Config.UUID);

            SelectedVersionLabel.Content = App.Lang.MainWindow.SelectedVersion;

            HeaderStartGame.Content = App.Lang.MainWindow.StartGame;
        }

        private void LoadMagnetBackgrounds(Image i)
        {
            /*
             Variables:

             Image i = Magnet icon image
             Button ii = Magnet background button
             Label IconLabel = Magnet text label
             */

            #region initialize Controls

            ContextMenu IconContextMenu = new ContextMenu();

            IconContextMenu.Items.Add("Resize (Large)");
            IconContextMenu.Items.Add("Hide");

            Button IconBackground = new Button()
            {
                Name = i.Name + "Background",
                Height = 150,
                Width = 150,
                BorderThickness = new Thickness(0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                //Effect = new System.Windows.Media.Effects.BlurEffect() { Radius = 20, KernelType = System.Windows.Media.Effects.KernelType.Gaussian },
                //This seems to only apply the blur to itself, excluding the background image
                Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/MagnetBackground.png"))),
                BorderBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/MagnetBackground_Hover.png"))),

                ContextMenu = IconContextMenu
            };

            IconBackground.PreviewMouseLeftButtonDown += Icon_MouseLeftButtonDown;
            IconBackground.PreviewMouseLeftButtonUp += Icon_MouseLeftButtonUp;
            IconBackground.PreviewMouseMove += Icon_MouseMove;

            this.RegisterName(i.Name + "Background", IconBackground);

            Panel.SetZIndex(IconBackground, -1);

            //End of button background
            //
            //Starting button label

            string LabelName = "";
            foreach (LanguageClass.LabelContentClass ii in App.Lang.LabelContents)
            {
                if (i.Name == ii.IconName)
                    LabelName = ii.LabelText;
            }

            Label IconLabel = new Label()
            {
                Name = i.Name + "Label",
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

            IconLabel.PreviewMouseLeftButtonDown += Icon_MouseLeftButtonDown;
            IconLabel.PreviewMouseLeftButtonUp += Icon_MouseLeftButtonUp;
            IconLabel.PreviewMouseMove += Icon_MouseMove;

            this.RegisterName(i.Name + "Label", IconLabel);

            #endregion


            //Calculates the X and Y cords based on the icons
            Point relativePoint = i.TransformToAncestor(this).Transform(new Point(0, 0));
            IconBackground.Margin = new Thickness(relativePoint.X - 55, relativePoint.Y - 55, 0, 0);
            IconLabel.Margin = new Thickness(relativePoint.X - 35, relativePoint.Y + 50, 0, 0);

            MainGrid.Children.Add(IconBackground);
            MainGrid.Children.Add(IconLabel);

            Backgrounds.Add(new BackgroundButtonClass()
            {
                BackgroundButton = IconBackground,
                Icon = i,
                Size = 1,
                TextLabel = IconLabel,
                Location = new Point(i.Margin.Left, i.Margin.Top)
            });

            //Removes the "taken" points
            IconPointClass.IconPoints.Remove(new Point(i.Margin.Left, i.Margin.Top));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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

            MessageBoxYesNo Form = new MessageBoxYesNo("Close Endless Launcher?", "Close", "Cancel");
            if (Form.ShowDialog() == true) {
                Application.Current.Shutdown();
            }
            else
            {
                CloseB.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Close.png")));
            }
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Icon_MouseEnter(object sender, MouseEventArgs e)
        {
            string imagename = (sender as Image).Name;
            var targetbutton = (Button)this.FindName(imagename + "Background");
            targetbutton.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/MagnetBackground_Hover.png")));
        }

        private void Icon_MouseLeave(object sender, MouseEventArgs e)
        {
            string imagename = (sender as Image).Name;
            var targetbutton = (Button)this.FindName(imagename + "Background");
            targetbutton.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/MagnetBackground.png")));
        }

        #region Magnet Drag Drop

        //
        // VARIABLES: 
        //
        private Boolean DraggingEnabled = false;    //Enables dragging / i.e. only allow dragging when in modifying mode
        private Boolean IsDragging = false;         //Prevents moving when not dragging
        private Image DraggedObject;                //Icon image
        private Button DraggedBackground;           //Button background
        private Label DraggedLabel;                 //Text label
        private Thickness OriginalPos;              //Original position to calculate new position


        private void Icon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DraggingEnabled)
            {
                IsDragging = true;

                if (sender is Button)
                {
                    string Sendername = ((Button)sender).Name;
                    DraggedObject = (Image)this.FindName(Sendername.Replace("Background", ""));

                    DraggedBackground = sender as Button;

                    DraggedLabel = (Label)this.FindName(Sendername.Replace("Background", "Label"));

                }
                if (sender is Label)
                {

                    string Sendername = ((Label)sender).Name;
                    DraggedObject = (Image)this.FindName(Sendername.Replace("Label", ""));

                    DraggedBackground = (Button)this.FindName(Sendername.Replace("Label", "Background"));

                    DraggedLabel = sender as Label;
                }
                if (sender is Image)
                {
                    DraggedObject = sender as Image;

                    DraggedBackground = (Button)this.FindName(DraggedObject.Name + "Background");

                    DraggedLabel = (Label)this.FindName(DraggedObject.Name + "Label");
                }

                OriginalPos = DraggedObject.Margin;

                //Adds the point back into the list so magnet can return
                IconPointClass.IconPoints.Add(Backgrounds.Find(x => x.BackgroundButton == DraggedBackground).Location);
            }
        }

        private void Icon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DraggingEnabled)
            {
                if (DraggedObject == null)
                {
                    return;
                }

                IsDragging = false;


                DraggedObject.Margin = PointDistance.GetClosestPoint(DraggedObject.Margin, IconPointClass.IconPoints, OriginalPos, DraggedBackground, DraggedLabel);

                int AddedBackgroundIndex = Backgrounds.FindIndex(x => x.BackgroundButton == DraggedBackground);
                Backgrounds[AddedBackgroundIndex].Location = new Point(DraggedObject.Margin.Left, DraggedObject.Margin.Top);

                //Removes the point since a magnet is already there
                IconPointClass.IconPoints.Remove(Backgrounds[AddedBackgroundIndex].Location);

                //Ends dragging
                DraggedObject = null;
            }
            else //not dragging i.e. is clicking
            {
                string ClickedName = "";

                if (sender is Image)
                    ClickedName = ((Image)sender).Name;

                if (sender is Button)
                    ClickedName = ((Button)sender).Name;

                if (sender is Label)
                    ClickedName = ((Label)sender).Name;

                if (ClickedName.Contains("GameSettings"))
                {
                    GameSettingsWindow Window = new GameSettingsWindow();
                    Window.ShowDialog();
                }
            }
            
        }

        private void Icon_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDragging && DraggedObject != null && DraggingEnabled)
            {

                DraggedObject.Margin = new Thickness(e.GetPosition(this).X - 20, e.GetPosition(this).Y - 20, 0, 0);
                DraggedBackground.Margin = new Thickness(e.GetPosition(this).X - 75, e.GetPosition(this).Y - 75, 0, 0);
                DraggedLabel.Margin = new Thickness(e.GetPosition(this).X - 55, e.GetPosition(this).Y + 30, 0, 0);
            }

        }

#endregion

        private void HeaderStartGame_Click(object sender, RoutedEventArgs e)
        {
            //Launch.LaunchGame("", 25565, Config.Version, "EN");
        }

        private void ModifyMagnets_Click(object sender, RoutedEventArgs e)
        {
            DraggingEnabled = !DraggingEnabled;     //Enables / disables dragging
        }
    }
}
