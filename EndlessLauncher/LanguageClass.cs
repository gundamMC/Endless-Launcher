using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndlessLauncher
{
    public class LanguageClass
    {
        /*
         * Structures
         */
        #region Class Structures
        public class ConnectionText
        {
            public string Connected { get; set; }

            public string Failed { get; set; }

            public string ConnectionLost { get; set; }
        }

        public class MainWindowClass
        {

            public string Title { get; set; }

            public string Hello { get; set; }

            public ConnectionText ConnectionText { get; set; }

            public string UUID { get; set; }

            public string SelectedVersion { get; set; }

            public string StartGame { get; set; }
        }

        public class LabelContentClass
        {
            public string IconName { get; set; }

            public string LabelText { get; set; }
        }

#endregion

        public MainWindowClass MainWindow { get; set; }

        public List<LabelContentClass> LabelContents = new List<LabelContentClass>()
        {
            new LabelContentClass(){ IconName = "GameSettings", LabelText = "Game Settings"},

            new LabelContentClass(){ IconName = "LauncherSettings", LabelText = "Settings"},

            new LabelContentClass(){ IconName = "ResourceAndShaderPacks", LabelText = "Graphics"},

            new LabelContentClass(){ IconName = "APIDownload", LabelText = "APIs"},

            new LabelContentClass(){ IconName = "ModManager", LabelText = "Mods"},

            new LabelContentClass(){ IconName = "VersionManager", LabelText = "Versions"},

            new LabelContentClass(){ IconName = "MapManager", LabelText = "Maps"},

            new LabelContentClass(){ IconName = "Support", LabelText = "Support"},

            new LabelContentClass(){ IconName = "Downloads", LabelText = "Downloads"},

            new LabelContentClass(){ IconName = "StartGame", LabelText = "Start Game"}
        };
    }
}
