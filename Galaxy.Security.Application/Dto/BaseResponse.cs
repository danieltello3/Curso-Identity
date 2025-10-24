using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.Security.Application.Dto
{
    public class BaseResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? ErrorCode { get; set; }  
        public T? Result { get; set; }


        public static BaseResponse<T> Success(T data, string message = "Operation completed success")
        {
            return new BaseResponse<T>
            {
                IsSuccess = true,
                Message = message,
                Result = data
            };
        }

        public static BaseResponse<T> Failure(string message, string? errorCode = null)
        {
            return new BaseResponse<T>
            {
                IsSuccess = false,
                Message = message,
                ErrorCode = errorCode
            };
        }
    }
}
