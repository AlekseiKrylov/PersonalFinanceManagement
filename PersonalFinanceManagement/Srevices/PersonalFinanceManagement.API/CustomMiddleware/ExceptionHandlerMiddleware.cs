﻿using PersonalFinanceManagement.BLL.Exceptions;
using PersonalFinanceManagement.Domain.APIModels;
using System.Net;
using System.Text.Json;

namespace PersonalFinanceManagement.API.CustomMiddleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            var errorMessage = "An internal server error occurred.";
            var response = httpContext.Response;
            response.ContentType = "application/json";

            switch (exception)
            {
                case TaskCanceledException:
                    response.StatusCode = 499;
                    errorMessage = "The request was canceled by the client.";
                    break;
                case UnauthorizedAccessException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorMessage = exception.Message;
                    break;
                case InvalidUserIdException:
                case ArgumentException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorMessage = exception.Message;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var errorResponse = new ApiError
            {
                Message = errorMessage,
                StatusCode = response.StatusCode,
            };

            await response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}