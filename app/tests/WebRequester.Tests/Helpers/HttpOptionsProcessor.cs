namespace WebRequester.Tests.Helpers
{
    using System.IO;
    using System.ServiceModel;
    using System.ServiceModel.Web;

    [ServiceContract]
    public class HttpOptionsProcessor
    {
        [WebInvoke(Method = "OPTIONS", UriTemplate = "/Options")]
        public Stream Options()
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
