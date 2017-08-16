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
        public static MojangAuthInfo GetAuthInfo()
        {

            if (String.IsNullOrWhiteSpace(GetLauncherConfig()))
            {
                return null;
            }

            JsonData ConfigData = JsonMapper.ToObject(GetLauncherConfig());

            string SelectedAccount = (string)ConfigData["selectedUser"]["account"];
            string SelectedProfile = (string)ConfigData["selectedUser"]["profile"];

            string SelectedVersionNameVar = (string)ConfigData["selectedProfile"];

            return new MojangAuthInfo()
            {
                Displayname = (string)ConfigData["authenticationDatabase"][SelectedAccount]["profiles"][SelectedProfile]["displayName"],

                Username = (string)ConfigData["authenticationDatabase"][SelectedAccount]["username"],

                UUID = Guid.Parse(SelectedProfile),

                ClientToken = Guid.Parse((string)ConfigData["clientToken"]),

                AccessToken = Guid.Parse((string)ConfigData["authenticationDatabase"][SelectedAccount]["accessToken"]),

                SelectedVersion = (string)ConfigData["profiles"][SelectedVersionNameVar]["lastVersionId"],

                SelectedVersionName = SelectedVersionNameVar
            };
        } 
    }
}
