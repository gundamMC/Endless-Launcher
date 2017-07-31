using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EndlessLauncher
{
    class GetRootPath
    {
        public static String RootPath()
        {
            //Default %appdata%\.minecraft
            if (String.IsNullOrWhiteSpace(App.Config.Directory))
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\.minecraft";
            }
            else
            {
                return App.Config.Directory;
            }
        }
    }
}
