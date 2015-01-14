using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LBS.DCT.JsonRPC.Requests
{
    public abstract class Request
    {
        public Request(string url, string id)
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

        private WebClient Client { get; set;  }
        public string Id { get; private set; }
        public string Url { get; private set; }
        protected  string Method { get; set; }
    }
}
