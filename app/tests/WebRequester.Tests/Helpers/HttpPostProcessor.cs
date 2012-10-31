namespace WebRequester.Tests.Helpers
{
    using System.IO;
    using System.ServiceModel;
    using System.ServiceModel.Web;

    [ServiceContract]
    public class HttpPostProcessor
    {
        [WebInvoke(Method = "POST", UriTemplate = "/OK")]
        public Stream Post()
        {
            var woc = WebOperationContext.Current;
            var ir = woc.IncomingRequest;
            var or = woc.OutgoingResponse;

            var uri = ir.UriTemplateMatch.RequestUri;
            or.StatusCode = System.Net.HttpStatusCode.OK;
            return null;
        }

        [WebInvoke(Method = "POST", UriTemplate = "/Error")]
        public Stream PostError()
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
