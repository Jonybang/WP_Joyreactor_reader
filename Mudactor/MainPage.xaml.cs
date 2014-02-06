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

namespace Mudactor
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Конструктор
        public MainPage()
        {
            InitializeComponent();

            // Задайте для контекста данных элемента управления listbox пример данных
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

        }

        // Загрузка данных для элементов ViewModel
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private static async void MakeRequest()
        {
            await UseHttpClient();
            UseWebClient();
        }

        private static async Task UseHttpClient()
        {
            Console.WriteLine("=== HttpClient ==");
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("http://www.google.com"));
            Console.WriteLine("HttpClient requesting...");
            var response = await client.SendAsync(request);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result.Substring(0, 100));
            Console.WriteLine("HttpClient done");
            HttpWebRequest myHttpWebRequest =
   (HttpWebRequest).Create("http://kbyte.ru");
            myHttpWebRequest.Proxy = new WebProxy("127.0.0.1", 8888);
            HttpWebResponse myHttpWebResponse =
              (HttpWebResponse)myHttpWebRequest.GetResponse();
        }

        private static void UseWebClient()
        {
            Console.WriteLine("=== WebClient ==");
            var webClient = new WebClient();
            webClient.DownloadStringAsync(new Uri("http://www.google.com"));
            Console.WriteLine("WebClient requesting...");
            webClient.DownloadStringCompleted += (sender, eventArgs) => Console.WriteLine(eventArgs.Result.Substring(0, 100));
            Console.WriteLine("WebClient done.");
        }
        public string send(string url, string par)
        {
            String secondStepForm3 = par;
            HttpWebRequest request3 = (HttpWebRequest)WebRequest.Create(url);
            request3.UserAgent = "Opera/9.80";
            request3.Method = "POST";
            request3.ContentType = "application/x-www-form-urlencoded";
            byte[] EncodedPostParams3 = Encoding.Default.GetBytes(secondStepForm3);
            request3.ContentLength = EncodedPostParams3.Length;
            request3.GetRequestStream().Write(EncodedPostParams3, 0, EncodedPostParams3.Length);
            request3.GetRequestStream().Close();
            HttpWebResponse response = (HttpWebResponse)request3.GetResponse();
            string lol = new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            return lol;
        }

    }
}