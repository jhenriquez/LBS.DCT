using Newtonsoft.Json.Linq;
using System;
using System.Net;
using Newtonsoft.Json;

namespace LBS.DCT.JsonRPC.Requests
{
    public abstract class Request
    {
        protected Request(string url, string id)
        {
            Client = new WebClient();

            if (String.IsNullOrEmpty(id))
            {
                throw new ArgumentException("id: can not be null or empty.");
            }

            if (String.IsNullOrEmpty(url))
            {
                throw new ArgumentException("id: can not be null or empty.");
            }

            Id = id;
            Url = url;
        }

        private Action<dynamic> ExecuteAsyncCallBack { get; set; }
        private WebClient Client { get; set; }
        public string Id { get; private set; }
        public string Url { get; private set; }
        protected string Method { get; set; }
        protected JToken Parameters { get; set; }

        public dynamic Execute()
        {
            return JsonConvert.DeserializeObject(Client.UploadString(new Uri(Url), "POST", BuildRequest()));
        }

        public void ExecuteAsync(Action<dynamic> cb)
        {
            ExecuteAsyncCallBack = cb;
            Client.UploadStringCompleted += Client_UploadStringCompleted;
            Client.UploadStringAsync(new Uri(Url), "POST", BuildRequest());
        }

        private void Client_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            ExecuteAsyncCallBack(JsonConvert.DeserializeObject(e.Result));
        }

        private String BuildRequest()
        {
            var request = new JObject();
            
            request["jsonrpc"] = "2.0";
            request["id"] = Id;
            request["method"] = Method;
            request["params"] = Parameters;

            return JsonConvert.SerializeObject(request);
        }
    }
}