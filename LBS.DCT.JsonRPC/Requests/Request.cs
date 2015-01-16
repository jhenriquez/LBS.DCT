using Newtonsoft.Json.Linq;
using System;
using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace LBS.DCT.JsonRPC.Requests
{
    public abstract class Request
    {

        private Action<dynamic> ExecuteAsyncCallBack { get; set; }
        private WebClient Client { get; set; }
        public string Id { get; private set; }
        public string Url { get; private set; }
        protected string Method { get; set; }
        protected JToken Parameters { get; set; }

        protected Request(string url, string id)
        {
            Client = new WebClient();

            if (String.IsNullOrEmpty(id))
            {
                throw new ArgumentException("id: can not be null or empty.");
            }

            if (String.IsNullOrEmpty(url))
            {
                throw new ArgumentException("Url: can not be null or empty.");
            }

            Id = id;
            Url = url;
        }

        public virtual dynamic Execute()
        {
            return JsonConvert.DeserializeObject(Client.UploadString(new Uri(Url), "POST", BuildRequest()));
        }

        public virtual void ExecuteAsync(Action<dynamic> cb)
        {
            Client.UploadStringCompleted += (s, e) => { cb( JsonConvert.DeserializeObject(e.Result)); };
            Client.UploadStringAsync(new Uri(Url), "POST", BuildRequest());
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