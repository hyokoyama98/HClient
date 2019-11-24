using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace HClient
{
    public class HClientResponse
    {
        protected HResponseService responseService;
        public HttpResponseMessage ResponseMessage;
        public string Content;
        public HttpStatusCode ResponseCode;
        public string ResponseCodeString;
        public List<string> SetCookies;

        public HClientResponse(HttpResponseMessage responseMessage)
        {
            responseService = new HResponseService();
            CreateInstance(responseMessage);
        }

        protected async void CreateInstance(HttpResponseMessage responseMessage)
        {
            if (responseMessage == null)
                return;

            ResponseMessage = responseMessage;
            Content = await ResponseMessage.Content.ReadAsStringAsync();
            ResponseCode = ResponseMessage.StatusCode;
            ResponseCodeString = ResponseMessage.StatusCode.ToString();
            SetCookies = responseService.GetSetCookies(ResponseMessage);
        }
    }
}