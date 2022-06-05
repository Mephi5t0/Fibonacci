using System.Net;
using View.Errors;

namespace Client
{
    public sealed class ClientResult<T>
    {
        public T Value { get; set; }

        public Error Error { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}