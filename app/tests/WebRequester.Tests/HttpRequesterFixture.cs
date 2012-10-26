namespace WebRequester.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Net;

    using NUnit.Framework;

    using SharpTestsEx;

    using WebRequester.Tests.Helpers;

    [TestFixture]
    public class HttpRequesterFixture
    {
        private HttpRequester requester;

        private WebRequesterHost host;

        [SetUp]
        public void SetUp()
        {
            this.requester = new HttpRequester();
            this.host = new WebRequesterHost();
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
            const string Uri = "http://localhost:5555/Get/NotFound";

            // Act:
            var response = this.requester.Get(Uri);

            // Assert:
            response.HttpStatusCode.Should().Be.EqualTo(HttpStatusCode.NotFound);
        }

        [Test]
        public void Get_ShouldReturn200WhenFoundResource()
        {
            // Arrange:
            const string Uri = "http://localhost:5555/Get/OK";

            // Act:
            var response = this.requester.Get(Uri);

            // Assert:
            response.HttpStatusCode.Should().Be.EqualTo(HttpStatusCode.OK);
        }

        [Test]
        public void Get_CanGetResourceWithOneParameter()
        {
            // Arrange:
            const string Uri = "http://localhost:5555/Get/OK";
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
            const string Uri = "http://localhost:5555/Get/OK";
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
            const string Uri = "http://localhost:5555/Get/OK";
            var parameters = new { q = "titans", include_entities = true };
            var headers = new Dictionary<string, string> { { "token", "token" } };

            // Act:
            var response = this.requester.Get(Uri, parameters, headers);

            // Assert:
            response.HttpStatusCode.Should().Be.EqualTo(HttpStatusCode.OK);
        }

        [Test]
        public void Post_ShouldReturn200WhenSendParameters()
        {
            // Arrange:
            const string Uri = "http://localhost:5555/Post";
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
            const string Uri = "http://localhost:5555/Post/Error";
            var parameters = new { carrier = "9", username = "0100000020", password = "1256" };
            var headers = new Dictionary<string, object> { { "error", 1 } };

            // Act:
            var response = this.requester.Post(Uri, parameters, headers);

            // Assert:
            response.HttpStatusCode.Should().Be.EqualTo(HttpStatusCode.MethodNotAllowed);
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
    }
}
