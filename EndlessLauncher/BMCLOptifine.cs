using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace EndlessLauncher
{
    class BMCLOptifineResult
    {

        public string Mcversion { get; set; }


        public string Type { get; set; }


        public string Patch { get; set; }


        public string Filename { get; set; }


        /* Apparently, these two items have no use at all,
         * so they are removed to optimize performance
         * 
         * public string _id { get; set; }

         * public int __v { get; set; }
         */
    }

    class BMCLOptifine
        
    {
        public List<BMCLOptifineResult> BMCLOptifineVersionList(string version = null)
        {
            Uri url;

            if (String.IsNullOrWhiteSpace(version))
            {  //If no version was specified
                url = new Uri("http://bmclapi2.bangbang93.com/optifine/versionList");
            }
            else
            {   //Get a certain version
                url = new Uri("http://bmclapi2.bangbang93.com/optifine/" + version);
            };

            WebClient csWebClient = new WebClient();
            string getData = csWebClient.DownloadString(url);
            LitJson.JsonData Data = LitJson.JsonMapper.ToObject(getData);

            int itemCnt = Data.Count;
            List<BMCLOptifineResult> Result = new List<BMCLOptifineResult>();

            for (int i = 0; i < itemCnt; i++)
            {   //Loops the java array due to the array not having a "name"
                //and therefore unable to map to an object

                Result.Add(new BMCLOptifineResult{

                    Mcversion = (string)Data[i]["MCversion"],
                    Type = (string)Data[i]["Type"],
                    Patch = (string)Data[i]["Patch"],
                    Filename = (string)Data[i]["Filename"]

                });
            }
            return Result;
        }
    }
}
