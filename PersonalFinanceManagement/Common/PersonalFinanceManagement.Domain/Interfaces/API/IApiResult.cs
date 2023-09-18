using PersonalFinanceManagement.Domain.APIModels;

namespace PersonalFinanceManagement.Domain.Interfaces.API
{
    public interface IApiResult
    {
        string GetErrorString { get; }
        bool IsSuccessful { get; }
        ApiResultStatus Status { get; }
    }
}