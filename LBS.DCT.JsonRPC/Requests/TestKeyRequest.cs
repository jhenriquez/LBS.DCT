using System;

namespace LBS.DCT.JsonRPC.Requests
{
    public class TestKeyRequest : Request
    {
        public String SessionKey { get; set; }

        public TestKeyRequest(String url) : base(url, Guid.NewGuid().ToString()) { Method = "testKey"; }
    }
}
