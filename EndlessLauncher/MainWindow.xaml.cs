using LitJson;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public List<BackgroundButtonClass> Icons = new List<BackgroundButtonClass>();
        //

        #region UI Methods

        public void InitializeText()
        {

            TitleLabel.Content = App.Lang.MainWindow.Title;

            HelloLabel.Content = App.Lang.MainWindow.Hello.Replace("{0}", App.Config.DisplayName);

            ConnectionLabel.Content = App.Lang.MainWindow.ConnectionText.Connected;    // add status check

            UUIDLabel.Content = App.Lang.MainWindow.UUID.Replace("{0}", App.Config.UUID.ToString());

            SelectedVersionLabel.Content = App.Lang.MainWindow.SelectedVersion;

            HeaderStartGame.Content = App.Lang.MainWindow.StartGame;
        }

        private void LoadMagnetConfig()
        {
            // default
            if (String.IsNullOrWhiteSpace(App.Config.Magnets))
            {
                foreach (Image i in ButtonsGrid.Children)
                {
                    Icons.Add(new BackgroundButtonClass(i, this, "small", -1));
                }
                return;
            }

            List<Image> toRemove = new List<Image>();

            foreach (Image i in ButtonsGrid.Children)
            {
                toRemove.Add(i);
            }

            foreach (string i in App.Config.Magnets.Split(';'))
            {
                String[] data = i.Split(',');
                // [0] = Image name
                // [1] = Size
                // [2] = Index
                // StartGame,0,1

                Image icon = (Image)this.FindName(data[0]);

                toRemove.Remove(icon);

                Icons.Add(new BackgroundButtonClass(icon, this, data[1], int.Parse(data[2])));
                
            }

            foreach (Image i in toRemove)
            {
                ButtonsGrid.Children.Remove(i);
            }

        }

        

        #endregion

        #region UI Builder

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            App.Config.Magnets = "StartGame,medium,0;GameSettings,small,5;LauncherSettings,small,6";

            LoadMagnetConfig();

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

#endregion

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


        public void Icon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
                IconPointClass.IconPoints[Icons.Find(x => x.BackgroundButton == DraggedBackground).Index].Used = false;
            }
        }

        public void Icon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DraggingEnabled)
            {
                if (DraggedObject == null)
                {
                    return;
                }

                IsDragging = false;

                DraggedObject.Margin = PointDistance.GetClosestPoint(DraggedObject.Margin, IconPointClass.IconPoints, OriginalPos, DraggedBackground, DraggedLabel);

                int AddedBackgroundIndex = Icons.FindIndex(x => x.BackgroundButton == DraggedBackground);
                Icons[AddedBackgroundIndex].Location = new Point(DraggedObject.Margin.Left, DraggedObject.Margin.Top);

                //Removes the point since a magnet is already there
                IconPointClass.IconPoints[Icons[AddedBackgroundIndex].Index].Used = false; ;

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

        public void Icon_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDragging && DraggedObject != null && DraggingEnabled)
            {

                DraggedObject.Margin = new Thickness(e.GetPosition(this).X - 20, e.GetPosition(this).Y - 20, 0, 0);

                DraggedBackground.Margin = new Thickness(e.GetPosition(this).X - 20 - Icons.Find(x => x.Icon == DraggedObject).BackgroundOffset, e.GetPosition(this).Y - 75, 0, 0);
                DraggedLabel.Margin = new Thickness(e.GetPosition(this).X - 20 - Icons.Find(x => x.Icon == DraggedObject).LabelOffset, e.GetPosition(this).Y + 30, 0, 0);
            }

        }

#endregion

        private void HeaderStartGame_Click(object sender, RoutedEventArgs e)
        {
            Launch.LaunchGame("", 0, App.Core.GetVersion(App.Config.Version), "EN");
        }

        private void ModifyMagnets_Click(object sender, RoutedEventArgs e)
        {
            DraggingEnabled = !DraggingEnabled;     //Enables / disables dragging
        }
    }
}
