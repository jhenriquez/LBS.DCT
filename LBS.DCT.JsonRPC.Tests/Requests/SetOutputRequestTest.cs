using System;
using System.Configuration;
using LBS.DCT.JsonRPC.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;
using System.Text;

namespace LBS.DCT.JsonRPC.Tests.Requests
{
    [TestClass]
    public class SetOutputRequestTest
    {
        private ChallengeRequest _challenge;
        private SessionKeyRequest _sessionKey;
        private SetOutputRequest _testSubject;

        [TestInitialize]
        public void BeforeEach()
        {
            _challenge = new ChallengeRequest("https://pegasus1.pegasusgateway.com/rpc/");
            _sessionKey = new SessionKeyRequest("https://pegasus1.pegasusgateway.com/rpc/");
            _testSubject = new SetOutputRequest("https://pegasus1.pegasusgateway.com/rpc/");
        }

        [TestMethod]
        public void it_should_throw_ArgumentNullException_when_the_required_parameters_are_not_provided()
        {
            ArgumentNullException exceptionThrown = null;

            try
            {
                _testSubject.Execute();
            }

            catch (ArgumentNullException ex)
            {
                exceptionThrown = ex;
            }

            Assert.IsNotNull(exceptionThrown, "Execute did NOT throw an exception.");
        }
    }
}
