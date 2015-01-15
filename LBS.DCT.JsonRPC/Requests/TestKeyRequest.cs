using System;
using Newtonsoft.Json.Linq;

namespace LBS.DCT.JsonRPC.Requests
{
    public class TestKeyRequest : Request
    {
        public String SessionKey { get; set; }

        public TestKeyRequest(String url) : base(url, Guid.NewGuid().ToString()) { Method = "testKey"; }

        public override dynamic Execute()
        {
            if (String.IsNullOrEmpty(SessionKey))
            {
                throw new InvalidOperationException("SessionKey was not provided.");
            }

            Parameters = new JArray(new [] { JValue.CreateString(SessionKey) });

            return base.Execute();
        }
    }
}
