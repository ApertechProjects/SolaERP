using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using SolaERP.Application.Constants;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace SolaERP.Application.Dtos.Shared
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public int StatusCode { get; set; }
        public object Errors { get; set; }
        public int TotalData { get; set; }
        public object ResultData { get; set; }

        public static ApiResponse<T> Success(T data, int statusCode = 200)
        {
            return new ApiResponse<T> { Data = data, StatusCode = statusCode };
        }
        public static ApiResponse<T> Success(T data, int statusCode, int totalData)
        {
            return new ApiResponse<T> { Data = data, StatusCode = statusCode, TotalData = totalData };
        }
        public static ApiResponse<T> Success(int statusCode)
        {
            return new ApiResponse<T> { StatusCode = statusCode };
        }

        public static ApiResponse<T> Success(string message)
        {
            return new ApiResponse<T> { StatusCode = 200, Errors = message };
        }

        private static T GetEmptyArrayIfCollection<T>()
        {
            if (typeof(T).GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>)))
            {
                var elementType = typeof(T).GetGenericArguments()[0];
                var collectionType = typeof(List<>).MakeGenericType(elementType);
                return (T)Activator.CreateInstance(collectionType);
            }

            return default(T);
        }


        public static ApiResponse<T> Fail(string error, int statusCode)
        {
            return new ApiResponse<T> { Errors = error, StatusCode = statusCode, Data = GetEmptyArrayIfCollection<T>() };
        }

        public static ApiResponse<T> Fail(object errors, int statusCode)
        {
            return new ApiResponse<T> { Errors = errors, StatusCode = statusCode };
        }

        public static ApiResponse<bool> Fail(string propertyName, string error, int statusCode)
        {
            var errorss = new Dictionary<string, string>
            {
                { char.ToLower(propertyName[0]) + propertyName.Substring(1), error }
            };

            return new ApiResponse<bool> { Errors = errorss, StatusCode = statusCode, Data = false };
        }


        public static ApiResponse<T> Fail(string propertyName, string error, int statusCode, bool isObject = true)
        {
            var errorss = new Dictionary<string, string>
            {
                { char.ToLower(propertyName[0]) + propertyName.Substring(1), error }
            };

            return new ApiResponse<T> { Errors = errorss, StatusCode = statusCode };
        }

        public static ApiResponse<T> Fail(List<string> propertyName, List<ModelErrorCollection> error, int statusCode)
        {
            var errorss = new Dictionary<string, string>();
            for (int i = 0; i < propertyName.Count; i++)
            {
                errorss.Add(char.ToLower(propertyName[i][0]) + propertyName[i].Substring(1), error[i][0].ErrorMessage);
            }

            return new ApiResponse<T>
            {
                Errors = errorss,
                StatusCode = statusCode
            };
        }

        public static ApiResponse<bool> CreateApiResponse(Func<T, bool> predicate, T data, int statusCode = 400, string erorrMessage = ResultMessageConstants.OperationUnsucces)
        {
            if (predicate(data))
                return new ApiResponse<bool> { Data = true, StatusCode = 200 };

            return new ApiResponse<bool> { Data = false, StatusCode = statusCode, Errors = erorrMessage };
        }
    }
}
