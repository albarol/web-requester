using System.Collections.Generic;

namespace WebRequester.Tests
{
    using System.Net;

    using NUnit.Framework;

    using SharpTestsEx;

    using WebRequester.Tests.Helpers;

    [TestFixture]
    public class HttpPostFixture
    {
        private HttpRequester requester;

        private WebRequesterHost host;

        [SetUp]
        public void SetUp()
        {
            this.requester = new HttpRequester();
            this.host = new WebRequesterHost(typeof(HttpPostProcessor));
            this.host.Open();
        }

        [TearDown]
        public void TearDown()
        {
            this.host.Close();
        }

        [Test]
        public void Post_ShouldReturn200WhenSendParameters()
        {
            // Arrange:
            const string Uri = "http://localhost:5555/OK";
            var parameters = new { carrier = "9", username = "0100000020", password = "123456" };

            // Act:
            var response = this.requester.Post(Uri, parameters);

            // Assert:
            response.HttpStatusCode.Should().Be.EqualTo(HttpStatusCode.OK);
        }

        [Test]
        public void Post_ShouldReturn405WhenSendIncorrectParameters()
        {
            // Arrange:
            const string Uri = "http://localhost:5555/Error";
            var parameters = new { carrier = "9", username = "0100000020", password = "1256" };
            var headers = new Dictionary<string, object> { { "error", 1 } };

            // Act:
            var response = this.requester.Post(Uri, parameters, headers);

            // Assert:
            response.HttpStatusCode.Should().Be.EqualTo(HttpStatusCode.MethodNotAllowed);
        }
    }
}
