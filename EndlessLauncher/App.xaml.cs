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
        public static RegistryConfig Config = JsonMapper.ToObject<RegistryConfig>(System.IO.File.ReadAllText(@"Some directory"));
        //Haven't decided  where the files will be stored yet


        // Initializes launcher core
        public static LauncherCore Core = LauncherCore.Create(
            new LauncherCoreCreationOption(
                gameRootPath: "",
                javaPath: GetJavaPath()
                )
            );


        private static String GetRootPath()
        {
            //Default %appdata%\.minecraft
            if (String.IsNullOrWhiteSpace(Config.Directory))
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\.minecraft";
            }
            else{
                return Config.Directory;
            }
        }
        private static String GetJavaPath()
        {
                 //Using try because the user may be using a 32-bit java under a 64-bit system
            try  //Prioritize 64bit
            {
                String javaKey = "SOFTWARE\\JavaSoft\\Java Runtime Environment";
                using (var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(javaKey))
                {
                    String currentVersion = baseKey.GetValue("CurrentVersion").ToString();
                    using (var homeKey = baseKey.OpenSubKey(currentVersion))
                        return homeKey.GetValue("JavaHome").ToString() + @"\bin\javaw.exe";
                }
            }
            catch //32bit
            {
                try
                {
                    String javaKey = "SOFTWARE\\Wow6432Node\\JavaSoft\\Java Runtime Environment";
                    using (var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(javaKey))
                    {
                        String currentVersion = baseKey.GetValue("CurrentVersion").ToString();
                        using (var homeKey = baseKey.OpenSubKey(currentVersion))
                            return homeKey.GetValue("JavaHome").ToString() + @"\bin\javaw.exe";
                    }
                }
                catch
                {
                    //No java path found
                    return "n/a";
                }
            }

        }
    }
}
