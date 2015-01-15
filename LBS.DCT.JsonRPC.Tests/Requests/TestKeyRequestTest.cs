using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using LBS.DCT.JsonRPC.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LBS.DCT.JsonRPC.Tests.Requests
{
    [TestClass]
    public class TestKeyRequestTest
    {
        private TestKeyRequest _testSubject;

        private ChallengeRequest _challangeChallengeRequest;
        private SessionKeyRequest _sessionKeyRequest;

        [TestInitialize]
        public void BeforeEach()
        {
            _challangeChallengeRequest = new ChallengeRequest("https://pegasus1.pegasusgateway.com/rpc/");
            _sessionKeyRequest = new SessionKeyRequest("https://pegasus1.pegasusgateway.com/rpc/");
            _testSubject = new TestKeyRequest("https://pegasus1.pegasusgateway.com/rpc/");
        }

        [TestMethod]
        public void it_should_throw_InvalidOperationException_when_the_key_is_not_provided()
        {
            InvalidOperationException exceptionThrown = null;

            try
            {
                _testSubject.Execute();
            }
            catch (InvalidOperationException ex)
            {
                exceptionThrown = ex;
            }

            Assert.IsNotNull(exceptionThrown, "Execute did NOT throw an exception.");
        }

        [TestMethod]
        public void it_should_return_a_result_property_with_value_ok_when_key_is_valid()
        {
            var challenge = _challangeChallengeRequest.Execute().result;
            var crypto = new SHA1CryptoServiceProvider();
            var secretKey = crypto.ComputeHash(Encoding.ASCII.GetBytes(String.Format("{0}{1}", challenge, ConfigurationManager.AppSettings["SecretKey"])));

            _sessionKeyRequest.Challenge = challenge;
            _sessionKeyRequest.HashedSecret = BitConverter.ToString(secretKey).Replace("-", "").ToLower();

            _testSubject.SessionKey = _sessionKeyRequest.Execute().result;

            var response = _testSubject.Execute();
            Assert.IsNotNull(response.result, response.ToString());
            Assert.AreEqual("ok", response.result.ToString());
        }
    }
}
