using KMCCC.Authentication;
using KMCCC.Launcher;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EndlessLauncher
{
    class Launch
    {
        public static void LaunchGame(string serverip, ushort port, KMCCC.Launcher.Version ver, string lang)
        {

            if (String.IsNullOrWhiteSpace(App.Config.DisplayName) || App.Config.AccessToken == null || App.Config.ClientToken == null)  
            {
                // this probably won't happen, but just be sure...
                MessageBoxOK MessageBox = new MessageBoxOK("Please re-login", "OK");
                MessageBox.ShowDialog();
                return;
            }

            if (ver == null)
                if (App.Core.GetVersions().First() != null)
                    ver = App.Core.GetVersions().First();
                else
                {
                    new MessageBoxOK("Failed to load versions, please download a version", "OK").ShowDialog();
                    return;
                }

            // Lost lib list
            List<string> lostFlie = new List<string>();

            lostFlie.Clear(); //Clear lost libs list, prevent re-launching causing problems

            LaunchOptions Options = new LaunchOptions
            {
                Version = ver,
                Authenticator = new YggdrasilRefresh(App.Config.AccessToken, App.Config.Twitch, App.Config.ClientToken),
            };

            if (App.Config.Maxram != 0)
                Options.MaxMemory = App.Config.Maxram;

            if (App.Config.Minram != 0)
                Options.MinMemory = App.Config.Minram;

            if (!String.IsNullOrWhiteSpace(serverip) && port != 0)
                Options.Server = new ServerInfo { Address = serverip, Port = port };


            if (!String.IsNullOrWhiteSpace(App.Config.WindowSize))
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

            LaunchResult result = App.Core.Launch(Options);
            //LAUNCH

            if (!result.Success)    // launch failed
            {
                MessageBoxOK form;

                switch (result.ErrorType)
                {
                    case ErrorType.NoJAVA:
                        form = new MessageBoxOK("Java error, try re-installing java", "OK");
                        form.ShowDialog();
                        break;
                    case ErrorType.AuthenticationFailed:
                        form = new MessageBoxOK("Authentication error, try re-logging in", "OK");
                        form.ShowDialog();
                        break;
                    case ErrorType.UncompressingFailed:
                        form = new MessageBoxOK("Game files missing or corrupted, try re-dowloading version", "OK");
                        form.ShowDialog();
                        break;
                    default:
                        form = new MessageBoxOK("Unexpected Error : " + result.ErrorMessage + " : " + result.Exception?.StackTrace, "OK");
                        form.ShowDialog();
                        break;
                }
            }
            if (result.Success == true)
            {
                //Successful launch, auto-close to-be implemented
            }
        }
    }
}
