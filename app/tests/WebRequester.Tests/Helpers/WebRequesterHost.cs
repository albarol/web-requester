namespace WebRequester.Tests.Helpers
{
    using System;
    using System.ServiceModel.Web;

    public class WebRequesterHost
    {
        private readonly WebServiceHost host;

        public WebRequesterHost()
        {
            this.host = new WebServiceHost(typeof(HttpProcessor), new Uri("http://localhost:5555"));
        }

        public void Open()
        {
            this.host.Open();
        }

        public void Close()
        {
            this.host.Close();
        }
    }
}
