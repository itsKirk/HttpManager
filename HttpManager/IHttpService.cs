
namespace HttpManager
{
    public interface IHttpService
    {
        Task<HttpMessenger<object>> Delete(string url);
        Task<HttpMessenger<T>> Get<T>(string url);
        Task<HttpMessenger<T>> GetAll<T>(string url);
        Task<HttpMessenger<TResponse>> Post<T, TResponse>(string url, T data);
        Task<HttpMessenger<object>> Post<T>(string url, T data);
        Task<HttpMessenger<object>> Put<T>(string url, T data);
    }
}