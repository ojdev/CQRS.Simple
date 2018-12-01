using System;

namespace CQRS.Api.Infrastructure.Filters
{
    [Serializable]
    public class ResponseWrap<TResult>
    {
        public bool Success { get; set; }
        public TResult Result { get; set; }
        public ErrorInfo Error { get; set; }
        public bool UnAuthorizedRequest { get; set; }
        public ResponseWrap()
        {
            Success = true;
        }

        public ResponseWrap(bool success)
        {
            Success = success;
        }

        public ResponseWrap(TResult result) : this()
        {
            Result = result;
        }

        public ResponseWrap(ErrorInfo error, bool unAuthorizedRequest = false)
        {
            Error = error;
            UnAuthorizedRequest = unAuthorizedRequest;
            Success = false;
        }
    }
    [Serializable]
    public class ResponseWrap : ResponseWrap<object>
    {
        public ResponseWrap() : base() { }
        public ResponseWrap(bool success) : base(success) { }
        public ResponseWrap(object result) : base(result) { }
        public ResponseWrap(ErrorInfo error, bool unAuthorizedRequest = false) : base(error, unAuthorizedRequest) { }
    }
    [Serializable]
    public class ValidationErrorInfo
    {
        public string Message { get; set; }
        public string[] Members { get; set; }

        public ValidationErrorInfo()
        {
        }

        public ValidationErrorInfo(string message) => Message = message;
        public ValidationErrorInfo(string message, string[] members) : this(message) => Members = members;
        public ValidationErrorInfo(string message, string member) : this(message, new[] { member }) { }
    }
    [Serializable]
    public class ErrorInfo
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public ValidationErrorInfo[] ValidationErrors { get; set; }

        public ErrorInfo()
        {
        }

        public ErrorInfo(string message)
        {
            Message = message;
        }

        public ErrorInfo(int code)
        {
            Code = code;
        }

        public ErrorInfo(int code, string message) : this(message)
        {
            Code = code;
        }

        public ErrorInfo(string message, string details) : this(message)
        {
            Details = details;
        }

        public ErrorInfo(int code, string message, string details) : this(message, details)
        {
            Code = code;
        }
    }
}
