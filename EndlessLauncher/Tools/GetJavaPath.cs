using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EndlessLauncher
{
    class GetJavaPath
    {
        public static String JavaPath()
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
