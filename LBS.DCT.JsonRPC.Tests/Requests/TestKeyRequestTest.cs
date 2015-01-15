using System;
using LBS.DCT.JsonRPC.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LBS.DCT.JsonRPC.Tests.Requests
{
    [TestClass]
    public class TestKeyRequestTest
    {
        private TestKeyRequest _testSubject;

        [TestInitialize]
        public void BeforeEach()
        {
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

            Assert.IsNotNull(exceptionThrown);
        }

        [TestMethod]
        public void it_should_return_a_result_property_with_value_ok_when_key_is_valid()
        {
            var response = _testSubject.Execute();
            Assert.IsNotNull(response.result);
        }
    }
}
