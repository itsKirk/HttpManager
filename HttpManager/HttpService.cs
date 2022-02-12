using System.Text;
using System.Text.Json;

namespace HttpManager
{
    /// <summary>
    ///     Provides a minified 0bject for sending HTTP requests and receiving HTTP responses from
    ///     a resource identified by a URI.    
    ///     
    /// </summary>
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _defaultSerializerOptions;

        public HttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _defaultSerializerOptions = new() { PropertyNameCaseInsensitive = true };
        }
        /// <summary>
        ///     Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">
        ///     The Type of request content sent to the server.
        /// </typeparam>
        /// <param name="url">
        ///     The Uri the request is sent to.
        /// </param>
        /// <param name="data">
        ///     The HTTP request content sent to the server.
        /// </param>
        /// <returns>
        ///     The task object representing the asynchronous operation.
        /// </returns>
        public async Task<HttpMessenger<object>> Post<T>(string url, T data)
        {
            var jsonData = JsonSerializer.Serialize(data);
            StringContent stringContent = new(jsonData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, stringContent);
            return new HttpMessenger<object>(null, response.IsSuccessStatusCode, response);
        }
        /// <summary>
        ///     Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">
        ///     The Type of request content sent to the server.
        /// </typeparam>
        /// <param name="url">
        ///     The Uri the request is sent to.
        /// </param>
        /// <param name="data">
        ///     The HTTP request content sent to the server.
        /// </param>
        /// <returns>
        ///     The task object representing the asynchronous operation.
        /// </returns>
        public async Task<HttpMessenger<object>> Put<T>(string url, T data)
        {
            var jsonData = JsonSerializer.Serialize(data);
            StringContent stringContent = new(jsonData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(url, stringContent);
            return new HttpMessenger<object>(null, response.IsSuccessStatusCode, response);
        }
        /// <summary>
        ///     Send a GET request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">
        ///     The Type of request content sent to the server.
        /// </typeparam>
        /// <param name="url">
        ///     The Uri the request is sent to.
        /// </param>
        /// <returns>
        ///     The task object representing the asynchronous operation.
        /// </returns>
        public async Task<HttpMessenger<T>> GetAll<T>(string url)
        {
            var responseHttp = await _httpClient.GetAsync(url);
            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await Deserialize<T>(responseHttp, _defaultSerializerOptions);
                return new HttpMessenger<T>(response, true, responseHttp);
            }
            else
            {
                return new HttpMessenger<T>(default, false, responseHttp);
            }
        }
        /// <summary>
        ///     Send a GET request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">
        ///     The Type of request content sent to the server.
        /// </typeparam>
        /// <param name="url">
        ///     The Uri the request is sent to.
        /// </param>
        /// <returns>
        ///     The task object representing the asynchronous operation.
        /// </returns>
        public async Task<HttpMessenger<T>> Get<T>(string url)
        {
            var responseHttp = await _httpClient.GetAsync(url);
            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await Deserialize<T>(responseHttp, _defaultSerializerOptions);
                return new HttpMessenger<T>(response, true, responseHttp);
            }
            else
            {
                return new HttpMessenger<T>(default, false, responseHttp);
            }
        }
        /// <summary>
        ///     Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">
        ///     The Type of request content sent to the server.
        /// </typeparam>
        /// <typeparam name="TResponse">
        ///     The Json representation of request content sent to the server.
        /// </typeparam>
        /// <param name="url">
        ///     The Uri the request is sent to.
        /// </param>
        /// <param name="data">
        ///     The HTTP request content sent to the server.
        /// </param>
        /// <returns>
        ///     The task object representing the asynchronous operation.
        /// </returns>
        public async Task<HttpMessenger<TResponse>> Post<T, TResponse>(string url, T data)
        {
            var jsonData = JsonSerializer.Serialize(data);
            StringContent stringContent = new(jsonData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, stringContent);
            if (response.IsSuccessStatusCode)
            {
                var responseDeserialized = await Deserialize<TResponse>(response, _defaultSerializerOptions);
                return new HttpMessenger<TResponse>(responseDeserialized, true, response);
            }
            else
            {
                return new HttpMessenger<TResponse>(default, false, response);
            }
        }
        /// <summary>
        ///     Reads the UTF-8 encoded text representing a single JSON value into a T.
        ///     The Stream will be read to completion.
        /// </summary>
        /// <typeparam name="T">
        ///     The type to deserialize the JSON value into.
        /// </typeparam>
        /// <param name="responseHttp">
        ///  Represents a HTTP response message including the status code and data.
        /// </param>
        /// <param name="options">
        ///   Provides options to be used with System.Text.Json.JsonSerializer.
        /// </param>
        /// <returns>
        ///     A T representation of the JSON value.
        /// </returns>
        private static async Task<T> Deserialize<T>(HttpResponseMessage responseHttp, JsonSerializerOptions options)
        {
            var responseString = await responseHttp.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseString, options);
        }
        /// <summary>
        ///     Send a DELETE request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="url">
        ///     The Uri the request is sent to.
        /// </param>
        /// <returns>
        ///     The task object representing the asynchronous operation.
        /// </returns>
        public async Task<HttpMessenger<object>> Delete(string url)
        {
            var responseHTTP = await _httpClient.DeleteAsync(url);
            return new HttpMessenger<object>(null, responseHTTP.IsSuccessStatusCode, responseHTTP);
        }
    }
}
