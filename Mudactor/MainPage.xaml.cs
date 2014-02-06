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
        NetworkCredential credentials = new NetworkCredential("<user>", "<password>", "<domain>");
        HttpWebRequest request = CreateWebRequest("<url>/_vti_bin/Webs.asmx", credentials);
        XDocument soapEnvelope = CreateSoapEnvelope("<GetWebCollection xmlns=\"http://schemas.microsoft.com/sharepoint/soap/\" />");
        static string soapEnvelope = @"<soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'><soap:Body></soap:Body></soap:Envelope>";

        // Конструктор
        public MainPage()
        {
            InitializeComponent();

            // Задайте для контекста данных элемента управления listbox пример данных
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelope, request);
        }

        // Загрузка данных для элементов ViewModel
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private static HttpWebRequest CreateWebRequest(string url, NetworkCredential credentials)
        {
            string action = "http://schemas.microsoft.com/sharepoint/soap/GetWebCollection";
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Credentials = credentials;
            req.Headers["SOAPAction"] = action;
            req.ContentType = "text/xml;charset=\"utf-8\"";
            req.Accept = "text/xml";
            req.Method = "POST";
            return req;
        }

        

        private static XDocument CreateSoapEnvelope(string content)
        {
            StringBuilder sb = new StringBuilder(soapEnvelope);
            sb.Insert(sb.ToString().IndexOf("</soap:Body>"), content);

            XDocument soapEnvelopeXml = XDocument.Parse(sb.ToString());

            return soapEnvelopeXml;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            webRequest.BeginGetRequestStream((IAsyncResult asynchronousResult) =>
            {
                HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
                Stream postStream = request.EndGetRequestStream(asynchronousResult);
                soapEnvelopeXml.Save(postStream);
                postStream.Close();

                request.BeginGetResponse(new AsyncCallback(GetResponseCallback), request);
            }, webRequest);
        }

        private static void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
            Stream streamResponse = response.GetResponseStream();
            StreamReader streamRead = new StreamReader(streamResponse);
            string responseString = streamRead.ReadToEnd();

            //do whatever with the response 

            streamResponse.Close();
            streamRead.Close();

            response.Close();
        }
    }
}