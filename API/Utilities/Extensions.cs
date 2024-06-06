using System.Net;

namespace API.Utilities
{

    public static class Extensions
    {
        public static async Task<ApiResult<T>> SafeApiCall<T>(this Task<T> task)
        {
            try
            {
                T okResult = await task;
                return new ApiResult<T>(okResult);
            }
            catch (HttpRequestException ex)
            {
                return new ApiResult<T>(new ServerOrNetworkNotAvailable(ex));
            }
            catch (UnhandledStatusCodeException ex)
            {
                return ex.StatusCode switch
                {
                    HttpStatusCode.InternalServerError => new ApiResult<T>(new ServerError(ex)),
                    _ => new ApiResult<T>(new OtherExceptionError(ex)),
                };
            }
        }
    }
    public class ApiResult<TOkResult>
    {
        public ApiResult(TOkResult okResult) => OkResult = okResult;
        public ApiResult(ApiExceptionResult exceptionResult) => ExceptionResult = exceptionResult;
        TOkResult? OkResult { get; }
        ApiExceptionResult? ExceptionResult { get; }

        public void Match(Action<TOkResult> whenOk, Action<ApiExceptionResult> whenException)
        {
            if (OkResult != null) whenOk(OkResult!);
            else whenException(ExceptionResult!);
        }
    }
    [Serializable]
    public class UnhandledStatusCodeException : Exception
    {

        public UnhandledStatusCodeException(HttpStatusCode statusCode) : base("", null)
        {
            StatusCode = statusCode;
        }
        protected UnhandledStatusCodeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        public HttpStatusCode StatusCode { get; }
    }
    public abstract class ApiExceptionResult { }
    public class ServerOrNetworkNotAvailable(HttpRequestException ex) : ApiExceptionResult
    {
        public HttpRequestException Ex { get; } = ex;
    }
    public abstract class UnhandledStatusCodeExceptionResult(UnhandledStatusCodeException ex) : ApiExceptionResult
    {
        public UnhandledStatusCodeException Ex { get; } = ex;
    }
    public class ServerError(UnhandledStatusCodeException ex) : UnhandledStatusCodeExceptionResult(ex) { }
    public class OtherExceptionError(UnhandledStatusCodeException ex) : UnhandledStatusCodeExceptionResult(ex) { }
}
