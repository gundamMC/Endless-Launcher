using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EndlessLauncher
{
    public class ConfigClass
    {
        #region PlayerOptions
        public string DisplayName { get; set; }


        public string Username { get; set; }


        public string UUID { get; set; }


        public string Directory { get; set; }


        public Guid Token { get; set; }


        public Guid CToken { get; set; }


        public Boolean Twitch { get; set; }


        public int Minram { get; set; }


        public int Maxram { get; set; }

        #endregion


        #region LauncherOptions
        public string WindowSize { get; set; }


        public string Magnets { get; set; }


        public string AutoClose { get; set; }


        public string Language { get; set; }


        public string Background {get;set;}


        public string DownloadSource { get; set; }


        public string Version { get; set; }


        #endregion
    }
}
