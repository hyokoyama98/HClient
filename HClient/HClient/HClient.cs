using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HClient
{
    class HClient
    {
        private HttpClient client;
        public HClientResponse hResponse;
        public HClientImgResponse hImgResponse;

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
            HttpClient client = _client;
            HttpContent hContent = null;

            client.DefaultRequestHeaders.Clear();
            if (_clientCookie != null && _clientCookie.CookieDictionary != null)
                foreach (var cookie in _clientCookie.CookieDictionary)
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Cookie", $"{cookie.Key}={cookie.Value}");

            if (_clientHeader != null)
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", _clientHeader.Accept);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Referer", _clientHeader.Referer);
                client.DefaultRequestHeaders.TryAddWithoutValidation("UserAgent", _clientHeader.UserAgent);
            }

            if (_clientHeader != null && _clientHeader.headersKeyValuePairs != null)
                foreach (var header in _clientHeader.headersKeyValuePairs)
                    client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);

            if (_json != null)
            {
                hContent = new StringContent(_json, Encoding.UTF8, "application/json");
            }
            else if (_json == null && _clientHeader != null && _clientHeader.postKeyValuePairs != null)
            {
                var param = "";
                foreach (var ss in _clientHeader.postKeyValuePairs)
                {
                    param += $"{ss.Key}={ss.Value}&";
                }
                hContent = new StringContent(param, Encoding.UTF8, "application/x-www-form-urlencoded");
            }

            return (client, hContent);
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
                hResponse.SetCookies = GetSetCookie(response);
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
                hImgResponse.SetCookies = GetSetCookie(response);
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
                hResponse.SetCookies = GetSetCookie(response);
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
                hResponse.SetCookies = GetSetCookie(response);
            }
            catch
            {
                hResponse.Content = null;
            }
            return hResponse;
        }

        public List<string> GetSetCookie(HttpResponseMessage message)
        {
            if (message == null || message.Headers == null)
                return null;

            var headerCollection = message.Headers;
            if (headerCollection.TryGetValues("Set-Cookie", out IEnumerable<string> values))
                return values.ToList();

            return null;
        }
    }
}