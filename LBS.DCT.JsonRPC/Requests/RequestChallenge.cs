using System;
using Newtonsoft.Json.Linq;

namespace LBS.DCT.JsonRPC.Requests
{
    public class RequestChallenge : Request
    {
        public RequestChallenge(String url)
            : base(url, Guid.NewGuid().ToString())
        {
            Method = "open";
            Parameters = new JArray(new [] { JValue.CreateNull(), JValue.CreateNull() });
        }
    }
}
