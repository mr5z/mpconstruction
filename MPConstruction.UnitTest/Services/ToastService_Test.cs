using MPConstruction.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPConstruction.UnitTest.Services
{
    internal class ToastService_Test
    {
        private IToastService service;

        class MockToastService : IToastService
        {
            public void Show(string format, params object[] values)
            {

            }
        }

        [SetUp]
        public void Setup()
        {
            service = new MockToastService();
        }

        [Test]
        public void TestShow()
        {
            var exc = new Exception("this");
            try
            {
                service.Show("");
                throw exc;
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.SameAs(exc));
            }
        }
    }
}
