using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ZJUWlan
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        public async Task<bool> SaveFile(string usr, string pwd)
        {
            try
            {
                StorageFolder storFolder = ApplicationData.Current.LocalFolder;
                StorageFile storfFile =
                    await storFolder.CreateFileAsync("info.txt", CreationCollisionOption.ReplaceExisting);
                storfFile = await storFolder.GetFileAsync("info.txt");
                await FileIO.WriteTextAsync(storfFile, (usr + "\r\n" + pwd));
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }


        public async Task<string> ReadFile()
        {
            string info =string.Empty ;
            try
            {
                StorageFolder loadFolder = ApplicationData.Current.LocalFolder;
                StorageFile loadFile = await loadFolder.GetFileAsync("info.txt");
                
                info = await FileIO.ReadTextAsync(loadFile);
                return info;
            }
            catch (Exception e)
            {
                e.ToString();
                return info;
            }
        } 



        private async void button_Click(object sender, RoutedEventArgs e)
        {

            string strID = textBox.Text;
            string strPassword = passwordBox .Password ;
            if(checkBox .IsChecked.Value  )
            {
                bool fileState = await SaveFile(strID, strPassword);
            }
            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData =
                "action=login&username=" + strID + "&password=" + strPassword + "&ac_id=3&type=1&mac=undefined&user_ip=&is_ldap=1&local_auth=1";
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
            var dialog = new Windows.UI.Popups.MessageDialog(content , "ZJUWlan");
            dialog.ShowAsync();
            myResponse.Dispose();
        }

        private async  void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            string info = string.Empty ;
            info =await  ReadFile();
            textBox.Text = info.Substring( 0,info.IndexOf("\r\n", StringComparison.Ordinal));
           passwordBox .Password  = info.Substring( info .IndexOf( "\r\n", StringComparison.Ordinal)+2,info.Length- (info.IndexOf("\r\n", StringComparison.Ordinal) + 2));
            
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Windows.UI.Popups.MessageDialog("由于ZJU自己都没有搞清楚怎么弄，该功能暂不开放。异地登陆即可注销当前","ZJUWlan");
            dialog.ShowAsync();
        }
    }
}
