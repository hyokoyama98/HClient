using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace HClient
{
    public class HContentService
    {
        public static HttpContent Create(string json)
        {
            HttpContent hContent = null;
            hContent = new StringContent(json, Encoding.UTF8, "application/json");

            return hContent;
        }

        public static HttpContent Create(HClientHeader _clientHeader)
        {
            if (_clientHeader == null || _clientHeader.postKeyValuePairs == null)
                return null;

            HttpContent hContent = null;
            var param = "";
            foreach (var ss in _clientHeader.postKeyValuePairs)
            {
                param += $"{ss.Key}={ss.Value}&";
            }
            hContent = new StringContent(param, Encoding.UTF8, "application/x-www-form-urlencoded");

            return hContent;
        }

        public static MultipartFormDataContent Create(HMultipart multipart)
        {
            var mContent = new MultipartFormDataContent(multipart.Boundary);
            var fileContent = new StreamContent(File.OpenRead(multipart.filePath));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(multipart.ContentType);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue(multipart.DispositionHeaderValue)
            {
                Name = multipart.DispositionHeaderName,
                FileName = Path.GetFileName(multipart.DispositionHeaderFileName)
            };
            mContent.Add(fileContent);

            return mContent;
        }
    }
}