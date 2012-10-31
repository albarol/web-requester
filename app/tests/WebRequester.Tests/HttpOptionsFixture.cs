namespace WebRequester.Tests
{
    using System.Net;

    using NUnit.Framework;

    using SharpTestsEx;

    using WebRequester.Tests.Helpers;

    [TestFixture]
    public class HttpOptionsFixture
    {
        private HttpRequester requester;

        private WebRequesterHost host;

        [SetUp]
        public void SetUp()
        {
            this.requester = new HttpRequester();
            this.host = new WebRequesterHost(typeof(HttpOptionsProcessor));
            this.host.Open();
        }

        [TearDown]
        public void TearDown()
        {
            this.host.Close();
        }

        [Test]
        public void Options_ShouldReturn200WhenGetOptions()
        {
            // Arrange:
            const string Uri = "http://localhost:5555/Options";

            // Act:
            var response = this.requester.Options(Uri);

            // Assert:
            response.HttpStatusCode.Should().Be.EqualTo(HttpStatusCode.OK);
        }

        [Test]
        public void Options_ShouldReturn200WhenGetOptionsWithHeaders()
        {
            // Arrange:
            const string Uri = "http://localhost:5555/Options";
            var headers = new { token = "token" };

            // Act:
            var response = this.requester.Options(Uri, headers);

            // Assert:
            response.HttpStatusCode.Should().Be.EqualTo(HttpStatusCode.OK);
        }
    }
}
