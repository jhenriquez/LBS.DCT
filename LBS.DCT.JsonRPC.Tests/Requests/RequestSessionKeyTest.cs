using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LBS.DCT.JsonRPC.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LBS.DCT.JsonRPC.Tests.Requests
{
    [TestClass]
    public class RequestSessionKeyTest
    {
        private RequestChallenge challangeRequest;
        private RequestSessionKey testSubject;

        [TestInitialize]
        public void BeforeEach()
        {
            challangeRequest = new RequestChallenge("https://pegasus1.pegasusgateway.com/rpc/");
            testSubject = new RequestSessionKey("https://pegasus1.pegasusgateway.com/rpc/");
        }

        [TestMethod]
        public void on_execute_it_should_throw_InvalidOperationException_when_challenge_not_provided()
        {
            InvalidOperationException exceptionThrown = null;

            try
            {
                testSubject.Execute();
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
                testSubject.Execute();
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
            testSubject.Challenge = "SomeChallenge";
            testSubject.HashedSecret = "Secret";
            Assert.IsInstanceOfType(testSubject.Execute(), typeof(IDynamicMetaObjectProvider));
        }

        [TestMethod]
        public void on_execute_it_returns_a_result_field_when_valid_values_provided()
        {
            var challenge = challangeRequest.Execute().result;
            var crypto = new SHA1CryptoServiceProvider();
            var secretKey =  crypto.ComputeHash(Encoding.ASCII.GetBytes(String.Format("{0}{1}", challenge, ConfigurationManager.AppSettings["SecretKey"])));

            testSubject.Challenge = challenge;
            testSubject.HashedSecret = BitConverter.ToString(secretKey).Replace("-", "").ToLower();

            var response = testSubject.Execute();
            Assert.IsNotNull(response.result, response.ToString());
            Assert.IsNull(response.someValue);
        }
    }
}
