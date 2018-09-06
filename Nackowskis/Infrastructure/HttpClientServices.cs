using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Nackowskis.Infrastructure
{
    public class HttpClientServices
    {
        public HttpClient client = new HttpClient();

        public HttpClientServices()
        {
            client.BaseAddress = new Uri("http://nackowskis.azurewebsites.net");
        }
    }
}
