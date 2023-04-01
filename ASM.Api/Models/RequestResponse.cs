using System.Runtime.InteropServices;

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

        public static RequestResponse GetError(string? message)
        {
            return new()
            {
                Message = message,
                Success = false
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
