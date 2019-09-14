using System.Collections.Generic;
using System.Net;

namespace HClient
{
    class HClientResponse
    {
        public string Content;
        public HttpStatusCode ResponseCode;
        public string ResponsCodeString;
        public List<string> SetCookies;
    }
}