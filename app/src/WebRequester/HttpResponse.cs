namespace WebRequester
{
    using System.Net;

    public class HttpResponse
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public string Body { get; set; }
    }
}
