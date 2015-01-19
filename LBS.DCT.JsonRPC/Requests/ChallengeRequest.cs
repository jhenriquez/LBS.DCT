using System;
using Newtonsoft.Json.Linq;

namespace Seiya.JsonRPC.Requests
{
    public class ChallengeRequest : Request
    {
        public ChallengeRequest(String url)
            : base(url, Guid.NewGuid().ToString())
        {
            Method = "open";
            Parameters = new JArray(new [] { JValue.CreateNull(), JValue.CreateNull() });
        }
    }
}
