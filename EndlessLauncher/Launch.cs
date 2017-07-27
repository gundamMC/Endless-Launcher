using KMCCC.Authentication;
using KMCCC.Launcher;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EndlessLauncher
{
    class Launch
    {
        public static void LaunchGame(string serverip, ushort port, KMCCC.Launcher.Version ver, string lang)
        {
            /*
             This code is taken from the "old" Endless Launcher that was written with winforms
             Please ignore all the messageboxes and a bunch of really bad code (I mean... it works... right?)
             I'm planning to rework this section...
                                                                   - gundamMC
             */

            if (App.Config.Username == "n/a")
            {
                //MessageBox.Show("Please log in", "Endless Launcher");
                return;
            }

            // Lost lib list
            List<string> lostFlie = new List<string>();

            lostFlie.Clear(); //Clear lost libs list, prevent re-launching causing problems

            Microsoft.Win32.RegistryKey key;
            key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\EndlessLauncher\Settings");
            key.SetValue("Version", ver.Id);
            key.Close();

            if (App.Config.Token.ToString() == "n/a") { /*MessageBox.Show("Error", "Please re-login");*/ return; }
            if (App.Config.CToken.ToString() == "n/a") { /*MessageBox.Show("Error", "Please re-login");*/ return; }

            LaunchOptions Options = new LaunchOptions
            {
                Version = ver,
                MaxMemory = App.Config.Maxram,
                MinMemory = App.Config.Minram,
                Authenticator = new YggdrasilRefresh(App.Config.Token, App.Config.Twitch, App.Config.CToken),
                Server = new ServerInfo { Address = serverip, Port = port },
            };

            if (App.Config.WindowSize != "n/a")
            {
                ushort Height = ushort.Parse(App.Config.WindowSize.Split(',')[0]);
                ushort Width = ushort.Parse(App.Config.WindowSize.Split(',')[1]);
                Options.Size = new WindowSize { Height = Height, Width = Width };
            }

            //Check for missing libs
            try
            {
                var libs = ver.Libraries.Select(lib => App.Core.GetLibPath(lib));

                var natives = ver.Natives.Select(native => App.Core.GetNativePath(native));
                foreach (string libflie in libs)
                {
                    if (!File.Exists(libflie))
                    {
                        //MessageBox.Show("Missing Libs：" + libflie);
                        lostFlie.Add(libflie);
                    }
                }

                foreach (string libflie in natives)
                {
                    if (!File.Exists(libflie))
                    {
                        lostFlie.Add(libflie);
                    }
                }
                if (lostFlie.Count > 0)
                {
                    //Download libs here, update after form creation
                    return;
                }
            }
            catch
            { }

            var result = App.Core.Launch(Options);
            //LAUNCH

            if (!result.Success)
            {
                switch (result.ErrorType)
                {
                    case ErrorType.NoJAVA:
                        //MessageBox.Show("Java error. Maybe try reinstalling Java?\nERROR：" + result.ErrorMessage, "Minecraft was not able to launch", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case ErrorType.AuthenticationFailed:
                        //MessageBox.Show("Unable to authorize the account. Please try resign-in.", "Minecraft was not able to launch", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case ErrorType.UncompressingFailed:
                        //MessageBox.Show("Game files missing or corrupted.\nPlease check your libraries files.\nERROR：" + result.ErrorMessage, "Minecraft was not able to launch", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    default:
                        //MessageBox.Show(result.ErrorMessage + "\n" +
                        //    (result.Exception?.StackTrace), "Unexpected Error:");
                        break;
                }
            }
            if (result.Success == true)
            {
                //Successful launch
            }
        }
    }
}
