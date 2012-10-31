namespace WebRequester.Tests.Helpers
{
    using System.IO;
    using System.ServiceModel;
    using System.ServiceModel.Web;

    [ServiceContract]
    public class HttpDeleteProcessor
    {
        [WebInvoke(Method = "DELETE", UriTemplate = "/Delete")]
        public Stream Delete()
        {
            var woc = WebOperationContext.Current;
            var ir = woc.IncomingRequest;
            var or = woc.OutgoingResponse;

            var uri = ir.UriTemplateMatch.RequestUri;
            or.StatusCode = System.Net.HttpStatusCode.OK;
            return null;
        }

        [WebInvoke(Method = "DELETE", UriTemplate = "/Delete/Error")]
        public Stream DeleteError()
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
