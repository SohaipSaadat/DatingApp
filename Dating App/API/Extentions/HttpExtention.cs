using DatingApplication.Helper;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DatingApplication.Extentions
{
    public static class HttpExtention
    {
        public static void AddPaginationHeader(this HttpResponse response, PaginationHeader header)
        {
            var opt = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            response.Headers.Add("Pagination", JsonSerializer.Serialize(header, opt));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
      
    }
}
