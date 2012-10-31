namespace WebRequester.Tests
{
    using System.Net;

    using NUnit.Framework;

    using SharpTestsEx;

    using WebRequester.Tests.Helpers;

    [TestFixture]
    public class HttpPutFixture
    {
        private HttpRequester requester;

        private WebRequesterHost host;

        [SetUp]
        public void SetUp()
        {
            this.requester = new HttpRequester();
            this.host = new WebRequesterHost(typeof(HttpPutProcessor));
            this.host.Open();
        }

        [TearDown]
        public void TearDown()
        {
            this.host.Close();
        }

        [Test]
        public void Put_ShouldReturn200WhenSendParameters()
        {
            // Arrange:
            const string Uri = "http://localhost:5555/Put";
            var parameters = new { carrier = "9", username = "0100000020", password = "123456" };

            // Act:
            var response = this.requester.Put(Uri, parameters);

            // Assert:
            response.HttpStatusCode.Should().Be.EqualTo(HttpStatusCode.OK);
        }

        [Test]
        public void Put_ShouldReturn405WhenSendIncorrectParameters()
        {
            // Arrange:
            const string Uri = "http://localhost:5555/Put/Error";
            var parameters = new { carrier = "9", username = "0100000020", password = "1256" };

            // Act:
            var response = this.requester.Put(Uri, parameters);

            // Assert:
            response.HttpStatusCode.Should().Be.EqualTo(HttpStatusCode.MethodNotAllowed);
        }
    }
}
