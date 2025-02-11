namespace MessageService.Core.Extentions
{
    public class WebResponse<T>
    {
        T? ResponeObject {get;set;}
        public bool IsSuccesed { get; set; } = true;
        public int StatusCode { get; set; } = 200;
        public string? ErrorMessage { get; set; } = null;
        public int TotalCount { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public WebResponse(string errorMessage, int statusCode)
        {
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
            IsSuccesed = false;
            
        }
        public WebResponse(T _ResponseObject, int totalCount, int limit)
        {
            ResponeObject = _ResponseObject;
            TotalCount = totalCount;
            TotalPages = totalCount/limit + 1;
        }
        public WebResponse(T responseObject)
        {
            ResponeObject = responseObject;
        }
    }
}
