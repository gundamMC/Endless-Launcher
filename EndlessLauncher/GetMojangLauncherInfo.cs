using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EndlessLauncher
{
    class GetMojangLauncherInfo
    {
        public class MojangAuthInfo
        {

            public string Displayname { get; set; }
            

            public string Username { get; set; }


            public Guid UUID { get; set; }


            public Guid ClientToken { get; set; }


            public Guid AccessToken { get; set; }


            public string SelectedVersion { get; set; }


            public string SelectedVersionName { get; set; }

        }

        public static string GetLauncherConfig()
        {
            string ConfigDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\.minecraft\launcher_profiles.json";
            if (File.Exists(ConfigDirectory))
            {
                return System.IO.File.ReadAllText(ConfigDirectory);
            }
            else
            {
                return null;
            }
        }


        ///<summary>
        ///Reads the config from Mojang's launcher
        ///</summary>
        public static MojangAuthInfo GetMojangAuthInfo()
        {

            if (String.IsNullOrWhiteSpace(GetLauncherConfig()))
            {
                return null;
            }

            JsonData ConfigData = JsonMapper.ToObject(GetLauncherConfig());

            string SelectedAccount = (string)ConfigData["selectedUser"]["account"];
            string SelectedProfile = (string)ConfigData["selectedUser"]["profile"];
            

            //Containing the basic info required to run Endless Launcher
            MojangAuthInfo returnclass =  new MojangAuthInfo()
            {
                Displayname = (string)ConfigData["authenticationDatabase"][SelectedAccount]["profiles"][SelectedProfile]["displayName"],

                Username = (string)ConfigData["authenticationDatabase"][SelectedAccount]["username"],

                UUID = Guid.Parse(SelectedProfile),

                ClientToken = Guid.Parse((string)ConfigData["clientToken"]),

                AccessToken = Guid.Parse((string)ConfigData["authenticationDatabase"][SelectedAccount]["accessToken"]),
            };


            #region Get last used profile
            List<string> profileNames = new List<string>();

            if (profileNames.Count > 0)
            {

                foreach (string key in ConfigData["profiles"].Keys)
                {
                    profileNames.Add(key);
                }

                string SelectedVersionNameVar = profileNames[0];
                DateTime LastUsedDate = DateTime.Parse((string)ConfigData["profiles"][SelectedVersionNameVar]["lastUsed"]);

                foreach (string i in profileNames)
                {
                    DateTime CurrentProfileDate = DateTime.Parse((string)ConfigData["profiles"][i]["lastUsed"]);
                    if (CurrentProfileDate > LastUsedDate)
                    {
                        SelectedVersionNameVar = i;
                        LastUsedDate = CurrentProfileDate;
                    }
                }

                returnclass.SelectedVersion = (string)ConfigData["profiles"][SelectedVersionNameVar]["lastVersionId"];
                returnclass.SelectedVersionName = SelectedVersionNameVar;
            }
#endregion


            return returnclass;
        } 
    }
}
