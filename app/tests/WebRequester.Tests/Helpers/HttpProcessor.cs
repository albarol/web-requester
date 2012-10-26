namespace WebRequester.Tests.Helpers
{
    using System;
    using System.IO;
    using System.ServiceModel;
    using System.ServiceModel.Web;

    [ServiceContract]
    public class HttpProcessor
    {
        [WebInvoke(Method = "GET", UriTemplate = "/Get/NotFound")]
        public Stream GetNotFound()
        {
            var woc = WebOperationContext.Current;
            var ir = woc.IncomingRequest;
            var or = woc.OutgoingResponse;

            var uri = ir.UriTemplateMatch.RequestUri;
            or.StatusCode = System.Net.HttpStatusCode.NotFound;
            return null;
        }

        [WebInvoke(Method = "GET", UriTemplate = "/Get/OK")]
        public Stream Ok()
        {
            var woc = WebOperationContext.Current;
            var ir = woc.IncomingRequest;
            var or = woc.OutgoingResponse;

            var uri = ir.UriTemplateMatch.RequestUri;
            or.StatusCode = System.Net.HttpStatusCode.OK;
            return null;
        }

        [WebInvoke(Method = "POST", UriTemplate = "/Post")]
        public Stream Post()
        {
            var woc = WebOperationContext.Current;
            var ir = woc.IncomingRequest;
            var or = woc.OutgoingResponse;

            var uri = ir.UriTemplateMatch.RequestUri;
            or.StatusCode = System.Net.HttpStatusCode.OK;
            return null;
        }

        [WebInvoke(Method = "POST", UriTemplate = "/Post/Error")]
        public Stream PostError()
        {
            var woc = WebOperationContext.Current;
            var ir = woc.IncomingRequest;
            var or = woc.OutgoingResponse;

            var uri = ir.UriTemplateMatch.RequestUri;
            or.StatusCode = System.Net.HttpStatusCode.MethodNotAllowed;
            return null;
        }

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