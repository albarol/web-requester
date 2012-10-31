namespace WebRequester.Tests
{
    using System.Net;

    using NUnit.Framework;

    using SharpTestsEx;

    using WebRequester.Tests.Helpers;

    [TestFixture]
    public class HttpDeleteFixture
    {
        private HttpRequester requester;

        private WebRequesterHost host;

        [SetUp]
        public void SetUp()
        {
            this.requester = new HttpRequester();
            this.host = new WebRequesterHost(typeof(HttpDeleteProcessor));
            this.host.Open();
        }

        [TearDown]
        public void TearDown()
        {
            this.host.Close();
        }

        [Test]
        public void Delete_ShouldReturn200WhenSendParameters()
        {
            // Arrange:
            const string Uri = "http://localhost:5555/Delete";
            var parameters = new { carrier = "9", username = "0100000020", password = "123456" };

            // Act:
            var response = this.requester.Delete(Uri, parameters);

            // Assert:
            response.HttpStatusCode.Should().Be.EqualTo(HttpStatusCode.OK);
        }

        [Test]
        public void Delete_ShouldReturn405WhenSendIncorrectParameters()
        {
            // Arrange:
            const string Uri = "http://localhost:5555/Delete/Error";
            var parameters = new { carrier = "9", username = "0100000020", password = "1256" };

            // Act:
            var response = this.requester.Delete(Uri, parameters);

            // Assert:
            response.HttpStatusCode.Should().Be.EqualTo(HttpStatusCode.MethodNotAllowed);
        }
    }
}
