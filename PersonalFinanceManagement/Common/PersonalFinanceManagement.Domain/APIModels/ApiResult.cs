namespace PersonalFinanceManagement.Domain.APIModels
{
    public enum ApiResultStatus
    {
        Success,
        Partial,
        Failure
    }

    public class ApiResult
    {
        public ApiResultStatus Status { get; }
        public string[] Errors { get; }
        public bool IsSuccessful => Status == ApiResultStatus.Success;
        public string GetErrorString => string.Join(", ", Errors) ?? "";

        public ApiResult(ApiResultStatus status, params string[] errors)
        {
            Status = status;
            Errors = errors.ToArray();
        }
    }

    public class ApiResult<T> : ApiResult
    {
        public T? Value { get; }

        public ApiResult(T value) : base(ApiResultStatus.Success)
        {
            Value = value;
        }

        public ApiResult(string error) : base(ApiResultStatus.Failure, error)
        {
            Value = default;
        }

        public ApiResult(ApiResultStatus status, string error) : base(status, error)
        {
            Value = default;
        }

        public ApiResult(T value, ApiResultStatus status, string error) : base(status, error)
        {
            Value = value;
        }
    }
}
