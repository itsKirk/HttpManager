using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpManager
{
    public class HttpMessenger<T>
    {
        public bool Success { get; set; }
        public T Response { get; set; }
        public HttpResponseMessage HttpResponseMessage { get; set; }
        public HttpMessenger(T response, bool success, HttpResponseMessage httpResponseMessage)
        {
            Success = success;
            Response = response;
            HttpResponseMessage = httpResponseMessage;
        }
        public async Task<string> GetBody()
        {
            if (HttpResponseMessage != null)
            {
                return await HttpResponseMessage.Content.ReadAsStringAsync();
            }
            return "";
        }
    }
}
