using System.Runtime.InteropServices;
using System.Security;

namespace ASM.Api.Models
{
    public class RequestResponse<T> : RequestResponse
    {
        public T? Data { get; set; }
    }

    public class RequestResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int Code { get; set; }

        public static RequestResponse GetError(string? message, int code = 0)
        {
            return new()
            {
                Message = message,
                Success = false,
                Code = code
            };
        }

        public static RequestResponse<T> GetSuccess<T>([Optional] T? data, [Optional] string? message)
        {
            return new()
            {
                Message = message,
                Data = data,
                Success = true
            };
        }

        public static RequestResponse GetSuccess([Optional] string? message)
        {
            return new()
            {
                Message = message,
                Success = true
            };
        }
    }
}
