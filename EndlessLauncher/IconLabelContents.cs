using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EndlessLauncher
{
    class IconLabelContents
    {
        public class LabelContentClass
        {
            public string IconName { get; set; }

            public string LabelText { get; set; }
        }

        public static List<LabelContentClass> LabelContents = new List<LabelContentClass>()
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
