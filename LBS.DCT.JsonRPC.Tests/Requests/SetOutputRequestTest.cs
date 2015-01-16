using System;
using System.Configuration;
using LBS.DCT.JsonRPC.Helpers;
using LBS.DCT.JsonRPC.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LBS.DCT.JsonRPC.Tests.Requests
{
    [TestClass]
    public class SetOutputRequestTest
    {
        private Authenticator _authenticator;
        private SetOutputRequest _testSubject;

        [TestInitialize]
        public void BeforeEach()
        {
            _authenticator = new Authenticator("https://pegasus1.pegasusgateway.com/rpc/", ConfigurationManager.AppSettings["SecretKey"]);
            _testSubject = new SetOutputRequest("https://pegasus1.pegasusgateway.com/rpc/");
        }

        [TestMethod]
        public void it_should_throw_InvalidOperationException_when_the_required_parameters_are_not_provided()
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
        public void it_should_return_a_result_property_with_value_ok_when_command_is_accepted()
        {
            _testSubject.SessionKey = _authenticator.Execute();
            _testSubject.ID = "356612024460380";
            _testSubject.OType = "e";
            _testSubject.OutputIndex = 1;
            _testSubject.State = true;

            Assert.AreEqual("True", _testSubject.Execute().result.ToString());
        }

        [TestMethod]
        public void it_should_return_a_result_property_with_value_ok_when_command_is_accepted_multiple_times()
        {
            _testSubject.SessionKey = _authenticator.Execute();
            _testSubject.ID = "356612024460380";
            _testSubject.OType = "e";
            _testSubject.OutputIndex = 1;
            _testSubject.State = true;

            var count = 10;
            while (count != 0)
            {
                Assert.AreEqual("True", _testSubject.Execute().result.ToString());
                count--;
            }            
        }
    }
}
