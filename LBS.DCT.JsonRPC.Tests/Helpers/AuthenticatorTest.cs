using System;
using System.Configuration;
using LBS.DCT.JsonRPC.Helpers;
using LBS.DCT.JsonRPC.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LBS.DCT.JsonRPC.Tests.Helpers
{
    [TestClass]
    public class AuthenticatorTest
    {
        private Authenticator _testSubject;
        private String url = "https://pegasus1.pegasusgateway.com/rpc/";
        private TestKeyRequest _testKeyRequest;
        
        [TestInitialize]
        public void BeforeEach()
        {
            _testSubject = new Authenticator(url, ConfigurationManager.AppSettings["SecretKey"]);
        }

        [TestMethod]
        public void constructor_defaults_Hash_should_be_null()
        {
            Assert.IsNull(_testSubject.Hash);
        }

        [TestMethod]
        public void constructor_can_take_a_Hash_Function_value()
        {
            _testSubject = new Authenticator(url, (x) =>  "Hassh");
            Assert.IsNotNull(_testSubject.Hash);
        }

        [TestMethod]
        public void constructor_can_take_a_SecretKey_value()
        {
            _testSubject = new Authenticator(url, "SecretKey");
            Assert.IsNotNull(_testSubject.SecretKey);
            Assert.AreEqual("SecretKey", _testSubject.SecretKey);
        }

        [TestMethod]
        public void execute_should_set_the_Challenge_property_along_the_way()
        {
            Assert.IsTrue(
                string.IsNullOrEmpty(_testSubject.Challenge)
                );

            _testSubject.Execute();

            Assert.IsFalse(
                string.IsNullOrEmpty(_testSubject.Challenge)
                );
        }

        [TestMethod]
        public void execute_should_return_a_valid_key()
        {
            var execute = _testSubject.Execute();

            Assert.IsFalse(String.IsNullOrEmpty(execute));

            _testKeyRequest = new TestKeyRequest(url);
            _testKeyRequest.SessionKey = execute;

            Assert.AreEqual("ok", _testKeyRequest.Execute().result.ToString());
        }
    }
}
