namespace WebRequester
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;

    public class HttpRequester
    {
        public HttpResponse Get(string uri)
        {
            return this.Get(uri, null);
        }

        public HttpResponse Get(string uri, object parameters)
        {
            return this.Get(uri, parameters, null);
        }

        public HttpResponse Get(string uri, object parameters, object headers)
        {
            var finalUri = string.Format("{0}{1}", uri, parameters.ToGetParameters());
            var webRequest = this.PrepareRequest(HttpMethod.Get, finalUri, headers);
            return this.GetResponse(webRequest);
        }

        public HttpResponse Post(string uri, object parameters)
        {
            return this.Post(uri, parameters, null);
        }

        public HttpResponse Post(string uri, object parameters, object headers)
        {
            var webRequest = this.PrepareRequest(HttpMethod.Post, uri, headers);
            webRequest.ContentType = "application/x-www-form-urlencoded";

            if (parameters != null)
            {
                byte[] data = parameters.ToPostParameters();
                webRequest.ContentLength = data.Length;

                using (Stream stream = webRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return this.GetResponse(webRequest);
        }

        public HttpResponse Put(string uri, object parameters)
        {
            return this.Put(uri, parameters, null);
        }

        public HttpResponse Put(string uri, object parameters, object headers)
        {
            var webRequest = this.PrepareRequest(HttpMethod.Put, uri, headers);
            webRequest.ContentType = "application/x-www-form-urlencoded";

            if (parameters != null)
            {
                byte[] data = parameters.ToPostParameters();
                webRequest.ContentLength = data.Length;

                using (Stream stream = webRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }

            return this.GetResponse(webRequest);
        }

        public HttpResponse Delete(string uri, object parameters)
        {
            return this.Delete(uri, parameters, null);
        }

        public HttpResponse Delete(string uri, object parameters, object headers)
        {
            var finalUri = string.Format("{0}{1}", uri, parameters.ToGetParameters());
            var webRequest = this.PrepareRequest(HttpMethod.Delete, finalUri, headers);
            return this.GetResponse(webRequest);
        }

        public HttpResponse Upload(string uri, FileStream stream)
        {
            return this.Upload(uri, stream, null);
        }

        public HttpResponse Upload(string uri, FileStream stream, object headers)
        {
            string boundary = string.Format("-------------------------{0}", DateTime.Now.Ticks.ToString("x"));
            byte[] boundaryBytes = Encoding.UTF8.GetBytes(string.Format("--{0}", boundary));

            const string BreakLine = "\r\n";
            byte[] breakLineBytes = Encoding.UTF8.GetBytes(BreakLine);

            var webRequest = this.PrepareRequest(HttpMethod.Post, uri, headers);
            webRequest.ServicePoint.Expect100Continue = false;
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            webRequest.Timeout = (int)TimeSpan.FromHours(24).TotalMilliseconds;
            webRequest.ReadWriteTimeout = (int)TimeSpan.FromSeconds(60).TotalMilliseconds;

            var headerStream = new MemoryStream();

            // --BOUNDARY
            headerStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            headerStream.Write(breakLineBytes, 0, BreakLine.Length);

            string fileHeader = string.Format(
                "Content-Disposition: form-data; name=\"fullfile\"; filename=\"{0}\"\r\nContent-Type: application/octet-stream; charset=UTF-8\r\nContent-Transfer-Encoding: binary\r\n",
                stream.Name);

            byte[] fileHeaderBytes = Encoding.UTF8.GetBytes(fileHeader);
            headerStream.Write(fileHeaderBytes, 0, fileHeaderBytes.Length);

            // SEPARATE
            headerStream.Write(breakLineBytes, 0, BreakLine.Length);

            long totalRequestLength = headerStream.Length + stream.Length + BreakLine.Length + boundaryBytes.Length
                                      + BreakLine.Length;

            webRequest.ContentLength = totalRequestLength;

            Stream outStream = webRequest.GetRequestStream();

            headerStream.WriteTo(outStream);
            headerStream.Close();

            byte[] buffer = new byte[4096];

            long remainsize = stream.Length;
            int bytesRead = 0;

            while ((bytesRead = stream.Read(buffer, 0, (int)((remainsize > buffer.Length) ? buffer.Length : remainsize))) != 0)
            {
                outStream.Write(buffer, 0, bytesRead);
                remainsize -= bytesRead;
            }

            stream.Close();

            outStream.Write(breakLineBytes, 0, BreakLine.Length);
            outStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            outStream.Write(breakLineBytes, 0, BreakLine.Length);

            return this.GetResponse(webRequest);

        }

        public HttpDownloadResponse Download(string uri)
        {
            return this.Download(uri, null, null);
        }

        public HttpDownloadResponse Download(string uri, object parameters)
        {
            return this.Download(uri, parameters, null);
        }

        public HttpDownloadResponse Download(string uri, object parameters, object headers)
        {
            try
            {
                var tempFile = Path.GetTempFileName();
                var finalUri = string.Format("{0}{1}", uri, parameters.ToGetParameters());
                var webRequest = this.PrepareRequest(HttpMethod.Get, finalUri, headers);
                webRequest.KeepAlive = true;
                webRequest.ServicePoint.Expect100Continue = false;
                webRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                var webResponse = (HttpWebResponse)webRequest.GetResponse();

                BinaryReader br = new BinaryReader(webResponse.GetResponseStream(), Encoding.UTF8);
                const int BufferSize = 1024 * 1024;
                byte[] buffer = new byte[BufferSize];
                Stream sw = new FileStream(tempFile, FileMode.Create);
                BinaryWriter bw = new BinaryWriter(sw);

                int bytesRead;
                while ((bytesRead = br.Read(buffer, 0, BufferSize)) > 0)
                {
                    bw.Write(buffer, 0, bytesRead);
                }
                bw.Close();
                sw.Close();
                br.Close();
                return new HttpDownloadResponse
                {
                    TemporaryFile = tempFile,
                    HttpStatusCode = (int)webResponse.StatusCode
                };
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError && e.Response != null)
                {
                    var resp = (HttpWebResponse)e.Response;
                    return new HttpDownloadResponse { HttpStatusCode = (int)resp.StatusCode };
                }
                return new HttpDownloadResponse { HttpStatusCode = (int)HttpStatusCode.ServiceUnavailable };
            }
        }

        private HttpWebRequest PrepareRequest(HttpMethod method, string uri, object headers)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.Method = method.ToString().ToUpper();
            foreach (var header in headers.ToDictionary())
            {
                webRequest.Headers.Add(header.Key, header.Value.ToString());
            }
            return webRequest;
        }

        private HttpResponse GetResponse(HttpWebRequest webRequest)
        {
            try
            {
                var webResponse = (HttpWebResponse)webRequest.GetResponse();
                return new HttpResponse
                {
                    HttpStatusCode = (int)webResponse.StatusCode,
                    Body = this.ReadBody(webResponse)
                };
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError && e.Response != null)
                {
                    var resp = (HttpWebResponse)e.Response;
                    return new HttpResponse { HttpStatusCode = (int)resp.StatusCode };
                }
                return new HttpResponse { HttpStatusCode = (int)HttpStatusCode.ServiceUnavailable };
            }
        }

        private string ReadBody(HttpWebResponse response)
        {
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
