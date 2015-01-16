using System;
using System.Security.Cryptography;
using System.Text;
using LBS.DCT.JsonRPC.Requests;

namespace LBS.DCT.JsonRPC.Helpers
{
    public class Authenticator
    {
        private ChallengeRequest ChallengeRequest { get; set; }
        private SessionKeyRequest SessionKeyRequest { get; set; }
        private Action<String> AsyncCallback { get; set; }

        public Func<String, String> Hash { get; set; }
        public String SecretKey { get; set; }
        public String Challenge { get; set; }

        public Authenticator(String url)
        {
            ChallengeRequest = new ChallengeRequest(url);
            SessionKeyRequest = new SessionKeyRequest(url);
        }

        public Authenticator(String url, String secret)
            : this(url)
        {
            SecretKey = secret;
        }

        public Authenticator(String url, Func<String, String> hasher)
            : this(url)
        {
            Hash = hasher;
        }

        public String Execute()
        {
            Challenge = SessionKeyRequest.Challenge = ChallengeRequest.Execute().result.ToString();

            SessionKeyRequest.HashedSecret = Hash != null ?  Hash(Challenge) : internalHasher();

            return SessionKeyRequest.Execute().result.ToString();
        }

        public void ExecuteAsync(Action<String> callback)
        {
        }

        private String internalHasher()
        {
            var crypto = new SHA1CryptoServiceProvider();
            var secretKey = crypto.ComputeHash(Encoding.ASCII.GetBytes(String.Format("{0}{1}", Challenge, SecretKey)));
            return BitConverter.ToString(secretKey).Replace("-", "").ToLower();
        }
    }
}