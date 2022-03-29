using System.Net;

namespace KeyCardWebServices.Exceptions;


[Serializable]
public class HttpException : Exception
{
    public HttpStatusCode HttpStatusCode { get; private set; }

    public HttpException(HttpStatusCode httpStatusCode) : base() { HttpStatusCode = httpStatusCode; }
    public HttpException(HttpStatusCode httpStatusCode, string message) : base(message) { HttpStatusCode = httpStatusCode; }
    public HttpException(HttpStatusCode httpStatusCode, string message, Exception inner) : base(message, inner) { HttpStatusCode = httpStatusCode; }
    protected HttpException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
