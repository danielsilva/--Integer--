using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace Web.Helpers
{
    public class WebApiHelper
    {
        Uri url;
        HttpClient client;

        public WebApiHelper(Uri url)
        {
            this.url = url;
            this.client = new HttpClient();
        }

        public T Get<T>(params string[] parameters) 
        {
            var urlParameters = new StringBuilder();
            foreach (string param in parameters)
            {
                string separator = urlParameters.Length == 0 ? "?" : "&";
                urlParameters.Append(separator + param);
            }

            HttpResponseMessage response;
            try
            {
                response = client.GetAsync(url + urlParameters.ToString()).Result;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
        }
    }
}