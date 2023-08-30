using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DatingApplication.Extentions
{
    public static class ClaimsPrincipalExtention
    {
        public static string GetUserName (this ClaimsPrincipal claimsPrincipal)
        {
           var userName = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value;
            return userName is null ? "" : userName;
        }
        public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var userId = int.TryParse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int num) ? num : 0;
            return userId;
        }
    }
}
