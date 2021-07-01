using System.Collections.Generic;

namespace ARchGLCloud.Application.Core
{
    public class ResponseResult<T> : ResponseResult
    {
        public T Item { get; set; }
    }

    public class ResponseResult
    {
        public bool Success { get; set; } = false;
        public int Code { get; set; } = 200;
        public static ResponseResult Error
        {
            get
            {
                return new ResponseResult()
                {
                    Success = false,
                    Code = 400
                };
            }
        }

    }

    public class ResponseMessage
    {
        public string message { get; set; } = "";
        public int code { get; set; } = 200;
        public int total { get; set; } = 0;
    }

    public class ResponseMessage<T> : ResponseMessage
    {
        public T data { get; set; }
    }

}
