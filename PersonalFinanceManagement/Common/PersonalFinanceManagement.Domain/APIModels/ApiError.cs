﻿namespace PersonalFinanceManagement.Domain.APIModels
{
    public class ApiError
    {
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }
    }
}
