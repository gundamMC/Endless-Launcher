using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.ComponentModel;

namespace EndlessLauncher
{

     


    class Internet
    {
        public static int[] Result { get; set; } = {0,0,0,0}; 

        public static int[] Download(string url, string localpath = null)
        {
            if (string.IsNullOrWhiteSpace(localpath))
            { //如果没有指定路径则默认下载到本地
                localpath = System.Environment.CurrentDirectory +@"\";
            }
            WebClient csWebClient = new WebClient();
            csWebClient.DownloadFileCompleted += (DownloadFileCompleted);
            csWebClient.DownloadProgressChanged += (DownloadProgressChanged);
            csWebClient.DownloadFileAsync(new Uri(url), localpath + System.IO.Path.GetFileName(url));
            return Result; //实际大小，已下载大小，百分比，下载是否完成(0=未完成,1=已完成) [int数组]
        }
       
        static void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs arg)
        {

            Result[0]= (int)arg.TotalBytesToReceive;
            Result[1] = (int)arg.BytesReceived;
            Result[2] = arg.ProgressPercentage;
        }

        static void DownloadFileCompleted(object sender, AsyncCompletedEventArgs arg)
        {
            if (arg.UserState != null)
            {Result[3]= 1;
            }
            else {Result[3]= 0; }
        }

    }
}

