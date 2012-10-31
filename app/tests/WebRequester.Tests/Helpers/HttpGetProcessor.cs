namespace WebRequester.Tests.Helpers
{
    using System.IO;
    using System.ServiceModel;
    using System.ServiceModel.Web;

    [ServiceContract]
    public class HttpGetProcessor
    {
        [WebInvoke(Method = "GET", UriTemplate = "/NotFound")]
        public Stream GetNotFound()
        {
            var woc = WebOperationContext.Current;
            var ir = woc.IncomingRequest;
            var or = woc.OutgoingResponse;

            var uri = ir.UriTemplateMatch.RequestUri;
            or.StatusCode = System.Net.HttpStatusCode.NotFound;
            return null;
        }

        [WebInvoke(Method = "GET", UriTemplate = "/OK")]
        public Stream Ok()
        {
            var woc = WebOperationContext.Current;
            var ir = woc.IncomingRequest;
            var or = woc.OutgoingResponse;

            var uri = ir.UriTemplateMatch.RequestUri;
            or.StatusCode = System.Net.HttpStatusCode.OK;
            return null;
        }
    }
}
