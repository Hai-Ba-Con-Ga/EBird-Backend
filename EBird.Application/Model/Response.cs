namespace Response
{
    public class Response<T>
    {
        public object Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public int? StatusCode { get; set; }

        /*
    Use these methods to set properties:  
    Examples: Response response = Response.builder
                                            .SetData(t)
                                            .SetStatusCode(HttpStatusCode.OK);  
*/

        public static Response<T> Builder() => new Response<T>();

        public dynamic SetData(T data)
        {
            Data = data;
            return this;
        }

        public dynamic SetSuccess(bool success)
        {
            Success = success;
            return this;
        }

        public dynamic SetMessage(string message)
        {
            Message = message;
            return this;
        }

        public dynamic SetStatusCode(int statusCode)
        {
            StatusCode = statusCode;
            return this;
        }

    }
}
