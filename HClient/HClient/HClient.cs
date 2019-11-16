using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HClient
{
    public class HClient
    {
        private HttpClient client;
        public HClientResponse hResponse;
        public HClientImgResponse hImgResponse;
        public HResponseService hResponseService;

        public HClient()
        {
            client = CreateInstance();
        }

        public HClient(HClientProxy _clientProxy)
        {
            client = CreateInstance(true, _clientProxy.wProxy);
        }

        private HttpClient CreateInstance(bool useProxy = false, WebProxy proxy = null)
        {
            hResponseService = new HResponseService();
            var handler = new HttpClientHandler()
            {
                UseCookies = false,
                UseProxy = useProxy,
                Proxy = proxy,
                AllowAutoRedirect = false,
            };
            var client = new HttpClient(handler);
            return client;
        }

        private (HttpClient, HttpContent) Create(HttpClient _client, HClientCookie _clientCookie, HClientHeader _clientHeader, string _json = null)
        {
            HttpClient client = null;
            HttpContent hContent = null;
            client = HClientService.Create(_client, _clientCookie, _clientHeader);
            if (_json != null)
                hContent = HContentService.Create(_json);
            else
                hContent = HContentService.Create(_clientHeader);
            return (client, hContent);
        }

        private (HttpClient, MultipartFormDataContent) Create(HttpClient _client, HClientCookie _clientCookie, HClientHeader _clientHeader, HMultipart multipart)
        {
            HttpClient client = null;
            MultipartFormDataContent content = null;
            client = HClientService.Create(_client, _clientCookie, _clientHeader);
            content = HContentService.Create(multipart);

            return (client, content);
        }

        public async Task<HClientResponse> Get(string _requestUrl, HClientCookie _clientCookie = null, HClientHeader _clientHeader = null)
        {
            try
            {
                hResponse = new HClientResponse();
                var instance = Create(client, _clientCookie, _clientHeader);
                client = instance.Item1;
                var response = await client.GetAsync(_requestUrl);
                hResponse.Content = await response.Content.ReadAsStringAsync();
                hResponse.ResponseCode = response.StatusCode;
                hResponse.ResponsCodeString = response.StatusCode.ToString();
                hResponse.SetCookies = hResponseService.GetSetCookies(response);
            }
            catch
            {
                hResponse.Content = null;
            }
            return hResponse;
        }

        public async Task<HClientImgResponse> GetImage(string _requestUrl, HClientCookie _clientCookie = null, HClientHeader _clientHeader = null)
        {
            try
            {
                hImgResponse = new HClientImgResponse();
                var instance = Create(client, _clientCookie, _clientHeader);
                client = instance.Item1;
                var response = await client.GetAsync(_requestUrl);
                var stream = await response.Content.ReadAsStreamAsync();
                hImgResponse.Content = null;
                hImgResponse.Image = Image.FromStream(stream);
                hImgResponse.ResponseCode = response.StatusCode;
                hImgResponse.ResponsCodeString = response.StatusCode.ToString();
                hImgResponse.SetCookies = hResponseService.GetSetCookies(response);
            }
            catch
            {
                hImgResponse.Image = null;
            }
            return hImgResponse;
        }

        public async Task<HClientResponse> Post(string _requestUrl, HClientCookie _clientCookie = null, HClientHeader _clientHeader = null)
        {
            try
            {
                hResponse = new HClientResponse();
                var instance = Create(client, _clientCookie, _clientHeader);
                client = instance.Item1;
                var content = instance.Item2;
                var response = await client.PostAsync(_requestUrl, content);
                hResponse.Content = await response.Content.ReadAsStringAsync();
                hResponse.ResponseCode = response.StatusCode;
                hResponse.ResponsCodeString = response.StatusCode.ToString();
                hResponse.SetCookies = hResponseService.GetSetCookies(response);
            }
            catch
            {
                hResponse.Content = null;
            }
            return hResponse;
        }

        public async Task<HClientResponse> Post(string _requestUrl, HClientCookie _clientCookie = null, HClientHeader _clientHeader = null, string _json = null)
        {
            try
            {
                hResponse = new HClientResponse();
                var instance = Create(client, _clientCookie, _clientHeader, _json);
                client = instance.Item1;
                var content = instance.Item2;
                var response = await client.PostAsync(_requestUrl, content);
                hResponse.Content = await response.Content.ReadAsStringAsync();
                hResponse.ResponseCode = response.StatusCode;
                hResponse.ResponsCodeString = response.StatusCode.ToString();
                hResponse.SetCookies = hResponseService.GetSetCookies(response);
            }
            catch
            {
                hResponse.Content = null;
            }
            return hResponse;
        }

        public async Task<HClientResponse> Post(string _requestUrl, HClientCookie _clientCookie = null, HClientHeader _clientHeader = null, HMultipart multipart = null)
        {
            try
            {
                hResponse = new HClientResponse();
                var instance = Create(client, _clientCookie, _clientHeader, multipart);
                client = instance.Item1;
                var content = instance.Item2;
                var response = await client.PostAsync(_requestUrl, content);
                hResponse.Content = await response.Content.ReadAsStringAsync();
                hResponse.ResponseCode = response.StatusCode;
                hResponse.ResponsCodeString = response.StatusCode.ToString();
                hResponse.SetCookies = hResponseService.GetSetCookies(response);
            }
            catch
            {
                hResponse.Content = null;
            }
            return hResponse;
        }
    }
}