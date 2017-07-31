using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using KMCCC.Launcher;
using Microsoft.Win32;
using System.IO;
using LitJson;

namespace EndlessLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //Sets global config
        public static RegistryConfig Config;


        // Initializes launcher core
        public static LauncherCore Core = LauncherCore.Create();
        
    }
}
