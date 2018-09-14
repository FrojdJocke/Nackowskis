using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Nackowskis.Infrastructure
{
    public static class HttpHelpers
    {

        public static List<T> ResponseToModelList<T>(HttpResponseMessage response, T returnType)
        {
            //var serializer = new DataContractJsonSerializer(typeof(List<T>));
            var responseStream = response.Content.ReadAsStreamAsync().Result;
            using (var streamReader = new StreamReader(responseStream))

            using (var jsonTextReader = new JsonTextReader(streamReader))
            {
                var jsonSerializer = new JsonSerializer();
                var model = jsonSerializer.Deserialize<List<T>>(jsonTextReader);
                return model;
            }
        }

        public static T ResponseToModel<T>(HttpResponseMessage response, T returnType)
        {
            //var serializer = new DataContractJsonSerializer(typeof(T));
            var responseStream = response.Content.ReadAsStreamAsync().Result;
            using (var streamReader = new StreamReader(responseStream))

            using (var jsonTextReader = new JsonTextReader(streamReader))
            {
                var jsonSerializer = new JsonSerializer();
                var model = jsonSerializer.Deserialize<T>(jsonTextReader);
                return model;
            }
        }
        
    }

}
