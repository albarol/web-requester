namespace WebRequester.Tests.Helpers
{
    using System.IO;
    using System.ServiceModel;
    using System.ServiceModel.Web;

    [ServiceContract]
    public class HttpPutProcessor
    {
        [WebInvoke(Method = "PUT", UriTemplate = "/Put")]
        public Stream Put()
        {
            var woc = WebOperationContext.Current;
            var ir = woc.IncomingRequest;
            var or = woc.OutgoingResponse;

            var uri = ir.UriTemplateMatch.RequestUri;
            or.StatusCode = System.Net.HttpStatusCode.OK;
            return null;
        }

        [WebInvoke(Method = "PUT", UriTemplate = "/Put/Error")]
        public Stream PutError()
        {
            var woc = WebOperationContext.Current;
            var ir = woc.IncomingRequest;
            var or = woc.OutgoingResponse;

            var uri = ir.UriTemplateMatch.RequestUri;
            or.StatusCode = System.Net.HttpStatusCode.MethodNotAllowed;
            return null;
        }
    }
}
