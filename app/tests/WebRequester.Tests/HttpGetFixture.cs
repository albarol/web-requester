namespace WebRequester.Tests
{
    using System.IO;
    using System.Net;

    using NUnit.Framework;

    using SharpTestsEx;

    using WebRequester.Tests.Helpers;

    [TestFixture]
    public class HttpGetFixture
    {
        private HttpRequester requester;
        private WebRequesterHost host;

        [SetUp]
        public void SetUp()
        {
            this.requester = new HttpRequester();
            this.host = new WebRequesterHost(typeof(HttpGetProcessor));
            this.host.Open();
        }

        [TearDown]
        public void TearDown()
        {
            this.host.Close();
        }

        [Test]
        public void Get_ShouldReturn404WhenNotFoundResource()
        {
            // Arrange:
            const string Uri = "http://localhost:5555/NotFound";

            // Act:
            var response = this.requester.Get(Uri);

            // Assert:
            response.HttpStatusCode.Should().Be.EqualTo(HttpStatusCode.NotFound);
        }

        [Test]
        public void Get_ShouldReturn200WhenFoundResource()
        {
            // Arrange:
            const string Uri = "http://localhost:5555/OK";

            // Act:
            var response = this.requester.Get(Uri);

            // Assert:
            response.HttpStatusCode.Should().Be.EqualTo(HttpStatusCode.OK);
        }

        [Test]
        public void Get_CanGetResourceWithOneParameter()
        {
            // Arrange:
            const string Uri = "http://localhost:5555/OK";
            var parameters = new { q = "titans" };

            // Act:
            var response = this.requester.Get(Uri, parameters);

            // Assert:
            response.HttpStatusCode.Should().Be.EqualTo(HttpStatusCode.OK);
        }

        [Test]
        public void Get_CanGetResourceWithManyParameters()
        {
            // Arrange:
            const string Uri = "http://localhost:5555/OK";
            var parameters = new { q = "titans", include_entities = true };

            // Act:
            var response = this.requester.Get(Uri, parameters);

            // Assert:
            response.HttpStatusCode.Should().Be.EqualTo(HttpStatusCode.OK);
        }

        [Test]
        public void Get_CanGetResourceWithHeaders()
        {
            // Arrange:
            const string Uri = "http://localhost:5555/OK";
            var parameters = new { q = "titans", include_entities = true };
            var headers = new { token = "token" };

            // Act:
            var response = this.requester.Get(Uri, parameters, headers);

            // Assert:
            response.HttpStatusCode.Should().Be.EqualTo(HttpStatusCode.OK);
        }

        [Test]
        public void Get_ShouldReturn503WhenResourceIsUnavaliable()
        {
            // Arrange:
            const string Uri = "http://qkmqekeqkmeqkek.net.as";

            // Act:
            var response = this.requester.Get(Uri);

            // Assert:
            response.HttpStatusCode.Should().Be.EqualTo(HttpStatusCode.ServiceUnavailable);
        }

        [Test]
        public void Get_ShouldReturnResponseHeaders()
        {
            // Arrange:
            const string Uri = "http://localhost:5555/OK";
            var parameters = new { q = "titans", include_entities = true };
            var headers = new { token = "token" };

            // Act:
            var response = this.requester.Get(Uri, parameters, headers);

            // Assert:
            response.Headers.Count.Should().Be.GreaterThan(0);
        }

        [Test]
        public void Download_CanDownloadFileFromUrl()
        {
            // Arrange
            const string Uri = "http://projects.developer.nokia.com/restfulplacesaround/browser/release_notes.txt";

            // Act:
            var download = this.requester.Download(Uri);

            // Assert:
            File.Exists(download.TemporaryFile).Should().Be.True();
            File.Delete(download.TemporaryFile);
        }

        [Test]
        public void Download_CanDownloadFileFromUrlWithParamters()
        {
            // Arrange
            const string Uri = "http://projects.developer.nokia.com/restfulplacesaround/browser/release_notes.txt";
            var parameters = new { version = "1.0" };

            // Act:
            var download = this.requester.Download(Uri, parameters);

            // Assert:
            File.Exists(download.TemporaryFile).Should().Be.True();
            File.Delete(download.TemporaryFile);
        }

        [Test]
        public void Download_ShouldReturn404WhenNotFoundResource()
        {
            // Arrange
            const string Uri = "http://localhost:5555";

            // Act:
            var download = this.requester.Download(Uri);

            // Assert:
            download.HttpStatusCode.Should().Be.EqualTo(HttpStatusCode.NotFound);
        }

        [Test]
        public void Download_ShouldReturn503WhenResourceIsUnavaliable()
        {
            // Arrange
            const string Uri = "http://qkmqekeqkmeqkek.net.as";

            // Act:
            var download = this.requester.Download(Uri);

            // Assert:
            download.HttpStatusCode.Should().Be.EqualTo(HttpStatusCode.ServiceUnavailable);
        }
    }
}
