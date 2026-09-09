using ApiCache.Helper.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiCache.Api.Filters
{
    public class ApiResponseWrapperFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if(context.Result is ObjectResult objectResult && objectResult.StatusCode >= 200 && objectResult.StatusCode < 300)
            {
                var rawData = objectResult.Value;

                if(rawData is not null && !IsAlreadyWrapped(rawData.GetType()))
                {
                    var wrapperType = typeof(ApiResponse<>).MakeGenericType(rawData.GetType());
                    var wrapperResponse = Activator.CreateInstance(wrapperType);

                    wrapperType.GetProperty("StatusCode")?.SetValue(wrapperResponse, objectResult.StatusCode);
                    wrapperType.GetProperty("Data")?.SetValue(wrapperResponse, rawData);

                    objectResult.Value = wrapperResponse;
                }
            }
            await next();
        }

        private static bool IsAlreadyWrapped(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ApiResponse<>))
                return true;

            return false;
        }
    }
}
