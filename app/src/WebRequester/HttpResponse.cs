namespace WebRequester
{
    using System.Collections.Generic;
    using System.Net;

    public class HttpResponse
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public string Body { get; set; }
    }
}
