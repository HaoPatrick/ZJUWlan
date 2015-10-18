using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows .ApplicationModel .Background ;


namespace AutoConnect
{
    public sealed class AutoConnect:IBackgroundTask
    {
        public async  void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral _deferral = taskInstance.GetDeferral();
            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData =
                "action=login&username=" + "4578" + "&password=" + "4564" + "&ac_id=3&type=1&mac=undefined&user_ip=&is_ldap=1&local_auth=1";
            byte[] data = encoding.GetBytes(postData);

            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("https://net.zju.edu.cn/cgi-bin/srun_portal");
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";


            Stream newStream = await myRequest.GetRequestStreamAsync();

            newStream.Write(data, 0, data.Length);
            newStream.Dispose();

            WebResponse myResponse = await myRequest.GetResponseAsync();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream());
            string content = reader.ReadToEnd();
            var dialog = new Windows.UI.Popups.MessageDialog(content, "ZJUWlan");
            await dialog.ShowAsync();
            myResponse.Dispose();
            _deferral.Complete();
        }
    }
}
