using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace SolaERP.Infrastructure.Dtos.Shared
{
    public class ApiResponse<T>
    {
        private ApiResponse()
        {
        }

        public T Data { get; set; }
        [JsonIgnore]
        public int StatusCode { get; set; }
        //public List<string> Errors { get; set; }
        public string Errors { get; set; }
        public static ApiResponse<T> Success(T data, int statusCode)
        {
            return new ApiResponse<T> { Data = data, StatusCode = statusCode };
        }
        public static ApiResponse<T> Success(int statusCode)
        {
            return new ApiResponse<T> { StatusCode = statusCode };
        }

        public static ApiResponse<T> Fail(string error, int statusCode)
        {
            return new ApiResponse<T> { Errors = error, StatusCode = statusCode };
        }

        public static ApiResponse<bool> Fail(string propertyName, string error, int statusCode)
        {
            var errorss = new Dictionary<string, string>
            {
                { propertyName, error }
            };

            var jsonResult = JsonConvert.SerializeObject(errorss);

            return new ApiResponse<bool> { Errors = jsonResult, StatusCode = statusCode, Data = false };
        }

        public static ApiResponse<T> Fail(List<string> propertyName, List<ModelErrorCollection> error, int statusCode)
        {
            var errorss = new Dictionary<string, string>();
            for (int i = 0; i < propertyName.Count; i++)
            {
                errorss.Add(propertyName[i], error[i][0].ErrorMessage);
            }

            string jsonResult = JsonConvert.SerializeObject(errorss);

            return new ApiResponse<T>
            {
                Errors = jsonResult,
                StatusCode = statusCode
            };
        }
    }
}
