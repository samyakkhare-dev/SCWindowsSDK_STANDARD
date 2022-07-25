using System.Net.Http;
using System.Threading.Tasks;
using SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.models;

namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.network
{
    public interface Network
    {
        Task<T> post<T>(HttpRequestMessage request, ErrorResponse errorResponse, DefaultErrorListener defaultErrorListener);
        Task<HttpResponseMessage> post(HttpRequestMessage request, ErrorResponse errorResponse, DefaultErrorListener defaultErrorListener);
        Task<T> get<T>(HttpRequestMessage request, ErrorResponse errorResponse, DefaultErrorListener defaultErrorListener);
        Task<HttpResponseMessage> get(HttpRequestMessage request, ErrorResponse errorResponse, DefaultErrorListener defaultErrorListener);
        Task<T> patch<T>(HttpRequestMessage request, ErrorResponse errorResponse, DefaultErrorListener defaultErrorListener);
        Task<HttpResponseMessage> patch(HttpRequestMessage request, ErrorResponse errorResponse, DefaultErrorListener defaultErrorListener);

        void close(HttpRequestMessage request);
    }
}
