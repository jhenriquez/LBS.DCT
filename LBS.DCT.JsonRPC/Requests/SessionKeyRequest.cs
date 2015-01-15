using System;
using Newtonsoft.Json.Linq;

namespace LBS.DCT.JsonRPC.Requests
{
    public class SessionKeyRequest : Request
    {
        public String Challenge { get; set; }
        public String HashedSecret { get; set; }

        public SessionKeyRequest(String url) : base(url, Guid.NewGuid().ToString())
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
