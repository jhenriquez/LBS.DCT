using System.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Seiya.JsonRPC.Requests;
using System.Threading;

namespace Seiya.JsonRPC.Tests.Requests
{
    [TestClass]
    public class RequestChallengeTest
    {
        private ChallengeRequest _subject;

        [TestInitialize]
        public void BeforeEach()
        {
            _subject = new ChallengeRequest("https://pegasus1.pegasusgateway.com/rpc/");
        }

        [TestMethod]
        public void on_execute_it_returns_a_dynamic_object()
        {
            var result = _subject.Execute();
            Assert.IsInstanceOfType(result, typeof (IDynamicMetaObjectProvider));
        }

        [TestMethod]
        public void on_execute_it_returns_a_result_field()
        {
            var result = _subject.Execute();
            Assert.IsNotNull(result.result);
            Assert.IsNull(result.someValue);
        }

        [TestMethod]
        public void executeAsync_provides_a_dynamic_result_throw_a_callback()
        {
            var done = new AutoResetEvent(false);

            _subject.ExecuteAsync((r) =>
            {
                Assert.IsFalse(true); // The test never reaches here.
            });

            Assert.IsFalse(true); // Fail the test!
            // TODO: Either change the method to use async or find a way to test call backs. Do we really want a callback? BTW, this method is working but we can't test it! So, use at your own risks :)
        }
    }
}