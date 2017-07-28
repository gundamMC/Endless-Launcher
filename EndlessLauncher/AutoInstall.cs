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
    

    class AutoInstall
        
    {
        static public string Get(string url) {  //抓取网页内容
            WebClient csWebClient = new WebClient();
            csWebClient.Credentials = CredentialCache.DefaultCredentials;

            Byte[] getData = csWebClient.DownloadData(url); 
            string getResult = Encoding.UTF8.GetString(getData); 

            return getResult;
        }


        class OptiFine {
            static object GetVersionList(string version=null)
            {
                string VersionList;
                if (version == null) {  //如果没有指定版本则抓取全版本列表
                    VersionList = Get("http://bmclapi2.bangbang93.com/optifine/versionList");
                }
                else{   //抓取指定版本列表
                    VersionList = Get("http://bmclapi2.bangbang93.com/optifine/" + version); 
                };
                LitJson.JsonData VersionArray = LitJson.JsonMapper.ToObject(VersionList);
                return VersionArray;
            }
        }
}
}
