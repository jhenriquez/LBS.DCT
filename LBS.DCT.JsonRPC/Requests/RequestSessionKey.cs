using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace LBS.DCT.JsonRPC.Requests
{
    public class RequestSessionKey : Request
    {
        public String Challenge { get; set; }
        public String HashedSecret { get; set; }

        public RequestSessionKey(String url) : base(url, Guid.NewGuid().ToString())
        {
            Method = "open";
        }

        public override dynamic Execute()
        {
            if (String.IsNullOrEmpty(Challenge))
            {
                throw new InvalidOperationException("Challenge was not provided.");
            }

            if (String.IsNullOrEmpty(HashedSecret))
            {
                throw new InvalidOperationException("HashedSecret was not provided.");
            }

            Parameters = new JArray(new [] { JValue.CreateString(Challenge), JValue.CreateString(HashedSecret) });
            return base.Execute();
        }
    }
}
