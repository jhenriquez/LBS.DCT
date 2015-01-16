using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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

        [TestMethod]
        public void execute_calls_the_provided_Hash_function_when_available()
        {
            var done = new AutoResetEvent(false);

            _testSubject.Hash = (c) =>
            {
                Assert.IsInstanceOfType(c, typeof(String)); // A String was passed to me.
                Assert.AreEqual(_testSubject.Challenge, c); // The challenge was passed as a parameter.

                var crypto = new SHA1CryptoServiceProvider();
                var secretKey = crypto.ComputeHash(Encoding.ASCII.GetBytes(String.Format("{0}{1}", c, _testSubject.SecretKey)));
                return BitConverter.ToString(secretKey).Replace("-", "").ToLower();
            };

            _testSubject.Execute();
            done.WaitOne(1500);
        }

        [TestMethod]
        public void executeAsync_provides_a_valid_key_through_a_callback()
        {
            var done = new AutoResetEvent(false);

            _testSubject.ExecuteAsync((key) => {

                Assert.IsTrue(false); // This point is never reached.

                Assert.IsInstanceOfType(key, typeof(String));

                _testKeyRequest = new TestKeyRequest(url);
                _testKeyRequest.SessionKey = key;

                Assert.AreEqual("ok", _testKeyRequest.Execute().result.ToString());

                done.Set();
            });

            Assert.IsFalse(true); // Fail the test on purpose.
            // TODO: Either change the method to use async or find a way to test call backs. Do we really want a callback? BTW, this method is working but we can't test it! So, use at your own risks :)
        }
    }
}
