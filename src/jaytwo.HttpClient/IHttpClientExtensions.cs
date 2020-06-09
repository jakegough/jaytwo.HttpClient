using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace jaytwo.HttpClient
{
    public static class IHttpClientExtensions
    {
        public static Task<HttpResponse> DeleteAsync(this IHttpClient httpClient, Action<HttpRequest> requestBuilder)
            => SendAsync(httpClient, HttpMethod.Delete, requestBuilder);

        public static Task<HttpResponse> DeleteAsync(this IHttpClient httpClient, Uri uri)
            => httpClient.DeleteAsync(x => x.WithUri(uri));

        public static Task<HttpResponse> DeleteAsync(this IHttpClient httpClient, string pathOrUri)
            => httpClient.DeleteAsync(x => x.WithUri(pathOrUri));

        public static Task<HttpResponse> DeleteAsync(this IHttpClient httpClient, string pathFormat, params string[] formatArgs)
            => httpClient.DeleteAsync(x => x.WithUri(pathFormat, formatArgs));

        public static Task<HttpResponse> DeleteAsync(this IHttpClient httpClient, string pathFormat, params object[] formatArgs)
            => httpClient.DeleteAsync(x => x.WithUri(pathFormat, formatArgs));

        public static Task<HttpResponse> GetAsync(this IHttpClient httpClient, Action<HttpRequest> requestBuilder)
            => SendAsync(httpClient, HttpMethod.Get, requestBuilder);

        public static Task<HttpResponse> GetAsync(this IHttpClient httpClient, Uri uri)
            => httpClient.GetAsync(x => x.WithUri(uri));

        public static Task<HttpResponse> GetAsync(this IHttpClient httpClient, string pathOrUri)
            => httpClient.GetAsync(x => x.WithUri(pathOrUri));

        public static Task<HttpResponse> GetAsync(this IHttpClient httpClient, string pathFormat, params string[] formatArgs)
            => httpClient.GetAsync(x => x.WithUri(pathFormat, formatArgs));

        public static Task<HttpResponse> GetAsync(this IHttpClient httpClient, string pathFormat, params object[] formatArgs)
            => httpClient.GetAsync(x => x.WithUri(pathFormat, formatArgs));

        public static Task<HttpResponse> HeadAsync(this IHttpClient httpClient, Action<HttpRequest> requestBuilder)
            => SendAsync(httpClient, HttpMethod.Head, requestBuilder);

        public static Task<HttpResponse> HeadAsync(this IHttpClient httpClient, Uri uri)
            => httpClient.HeadAsync(x => x.WithUri(uri));

        public static Task<HttpResponse> HeadAsync(this IHttpClient httpClient, string pathOrUri)
            => httpClient.HeadAsync(x => x.WithUri(pathOrUri));

        public static Task<HttpResponse> HeadAsync(this IHttpClient httpClient, string pathFormat, params string[] formatArgs)
            => httpClient.HeadAsync(x => x.WithUri(pathFormat, formatArgs));

        public static Task<HttpResponse> HeadAsync(this IHttpClient httpClient, string pathFormat, params object[] formatArgs)
            => httpClient.HeadAsync(x => x.WithUri(pathFormat, formatArgs));

        public static Task<HttpResponse> OptionsAsync(this IHttpClient httpClient, Action<HttpRequest> requestBuilder)
            => SendAsync(httpClient, HttpMethod.Options, requestBuilder);

        public static Task<HttpResponse> PatchAsync(this IHttpClient httpClient, Action<HttpRequest> requestBuilder)
            => SendAsync(httpClient, new HttpMethod("PATCH"), requestBuilder);

        public static Task<HttpResponse> PostAsync(this IHttpClient httpClient, Action<HttpRequest> requestBuilder)
            => SendAsync(httpClient, HttpMethod.Post, requestBuilder);

        public static Task<HttpResponse> PutAsync(this IHttpClient httpClient, Action<HttpRequest> requestBuilder)
            => SendAsync(httpClient, HttpMethod.Put, requestBuilder);

        public static Task<HttpResponse> TraceAsync(this IHttpClient httpClient, Action<HttpRequest> requestBuilder)
            => SendAsync(httpClient, HttpMethod.Trace, requestBuilder);

        public static Task<HttpResponse> SendAsync(this IHttpClient httpClient, Action<HttpRequest> requestBuilder)
        {
            var request = new HttpRequest();
            requestBuilder.Invoke(request);
            return httpClient.SendAsync(request);
        }

        private static Task<HttpResponse> SendAsync(IHttpClient httpClient, HttpMethod method, Action<HttpRequest> requestBuilder)
        {
            return httpClient.SendAsync(request =>
            {
                request.WithMethod(method);
                requestBuilder.Invoke(request);
            });
        }
    }
}
