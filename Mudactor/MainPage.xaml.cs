using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml.Linq;

namespace Mudactor
{
    public partial class MainPage : PhoneApplicationPage
    {
        string AccessToken = "";
        // Конструктор
        public MainPage()
        {
            InitializeComponent();

            // Задайте для контекста данных элемента управления listbox пример данных
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

            var url = "http://m.joyreactor.cc/login";
            Browser.Navigate(new Uri(url));
        }

        // Загрузка данных для элементов ViewModel
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

        }        

        private void Browser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
             if (e.Uri.IsAbsoluteUri)
            {
                string code = e.Uri.Query.ToString();
                string[] split = code.Split(new Char[] { '=' });
                string codeString = split.GetValue(0).ToString();
                string codeValue = split.GetValue(1).ToString();
                if (codeValue.Length > 0)
                {
                    var url = "http://joyreactor.cc/login" + codeValue;
 
                    //call access token
                    WebRequest request = WebRequest.Create(url); //FB access token Link
                    request.BeginGetResponse(new AsyncCallback(this.ResponseCallback_AccessToken), request);
                }
            }
            else
                return;
        }

        private void ResponseCallback_AccessToken(IAsyncResult asynchronousResult)
        {
            try
            {
                var request = (HttpWebRequest)asynchronousResult.AsyncState;
                using (var resp = (HttpWebResponse)request.EndGetResponse(asynchronousResult))
                {
                    using (var streamResponse = resp.GetResponseStream())
                    {
                        using (var streamRead = new StreamReader(streamResponse))
                        {
                            string responseString = streamRead.ReadToEnd();
                            string[] splitAccessToken = responseString.Split(new Char[] { '=' });
                            string accessTokenString = splitAccessToken.GetValue(0).ToString();
                            string accessTokenValue = splitAccessToken.GetValue(1).ToString();
                            if (accessTokenString == "access_token")
                            {
                                AccessToken = accessTokenValue;
                            }
                        }
                    }
                }
            }
            catch (WebException ex)
            {

            }
            GetAccessToken();
        }

        void GetAccessToken()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                if (string.IsNullOrEmpty(AccessToken))
                {
                    MessageBox.Show("AccessToken not valid");
                }
                else
                {
                    MessageBox.Show("AccessToken= " + AccessToken);
                    //LoadUserProfile();
                }
            });
        }
    }
}