using System;
using System.Configuration;
using System.Dynamic;
using System.Security.Cryptography;
using System.Text;
using LBS.DCT.JsonRPC.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LBS.DCT.JsonRPC.Tests.Requests
{
    [TestClass]
    public class RequestSessionKeyTest
    {
        private ChallengeRequest _challangeChallengeRequest;
        private SessionKeyRequest _testSubject;

        [TestInitialize]
        public void BeforeEach()
        {
            _challangeChallengeRequest = new ChallengeRequest("https://pegasus1.pegasusgateway.com/rpc/");
            _testSubject = new SessionKeyRequest("https://pegasus1.pegasusgateway.com/rpc/");
        }

        [TestMethod]
        public void on_execute_it_should_throw_InvalidOperationException_when_challenge_not_provided()
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

            Assert.IsNotNull(exceptionThrown);
        }

        [TestMethod]
        public void on_execute_it_should_throw_InvalidOperationException_when_hashedSecret_not_provided()
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

            Assert.IsNotNull(exceptionThrown);
        }

        [TestMethod]
        public void on_execute_returns_a_dynamic_object()
        {
            _testSubject.Challenge = "SomeChallenge";
            _testSubject.HashedSecret = "Secret";
            Assert.IsInstanceOfType(_testSubject.Execute(), typeof(IDynamicMetaObjectProvider));
        }

        [TestMethod]
        public void on_execute_it_returns_a_result_field_when_valid_values_provided()
        {
            var challenge = _challangeChallengeRequest.Execute().result;
            var crypto = new SHA1CryptoServiceProvider();
            var secretKey =  crypto.ComputeHash(Encoding.ASCII.GetBytes(String.Format("{0}{1}", challenge, ConfigurationManager.AppSettings["SecretKey"])));

            _testSubject.Challenge = challenge;
            _testSubject.HashedSecret = BitConverter.ToString(secretKey).Replace("-", "").ToLower();

            var response = _testSubject.Execute();
            Assert.IsNotNull(response.result, response.ToString());
            Assert.IsNull(response.someValue);
        }
    }
}
