namespace WebRequester.Tests
{
    using System.Net;

    using NUnit.Framework;

    using SharpTestsEx;

    using WebRequester.Tests.Helpers;

    [TestFixture]
    public class HttpHeadFixture
    {
        private HttpRequester requester;

        private WebRequesterHost host;

        [SetUp]
        public void SetUp()
        {
            this.requester = new HttpRequester();
            this.host = new WebRequesterHost(typeof(HttpHeadProcessor));
            this.host.Open();
        }

        [TearDown]
        public void TearDown()
        {
            this.host.Close();
        }

        [Test]
        public void Head_ShouldReturn200WhenGetHead()
        {
            // Arrange:
            const string Uri = "http://localhost:5555/Head";

            // Act:
            var response = this.requester.Head(Uri);

            // Assert:
            response.HttpStatusCode.Should().Be.EqualTo(HttpStatusCode.OK);
        }

        [Test]
        public void Head_ShouldReturn200WhenGetHeadWithHeaders()
        {
            // Arrange:
            const string Uri = "http://localhost:5555/Head";
            var headers = new { token = "token" };

            // Act:
            var response = this.requester.Head(Uri, headers);

            // Assert:
            response.HttpStatusCode.Should().Be.EqualTo(HttpStatusCode.OK);
        }
    }
}
