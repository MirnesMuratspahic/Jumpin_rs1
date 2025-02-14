using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Jumpin.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Jumpin.Services
{
    public class TokenHelperService : ITokenHelperService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenHelperService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private string ExtractTokenFromHeaders()
        {
            var authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer"))
            {
                return "";
            }
            return authHeader.Substring("Bearer ".Length);
        }

        private string ExtractEmailFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);

                var email = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;

                if (!string.IsNullOrEmpty(email))
                {
                    return email;
                }

                return "";
            }
            return "";
        }

        public async Task<string> GetEmailFromToken()
        {
            return await Task.Run(() =>
            {
                var token = ExtractTokenFromHeaders();
                if(!string.IsNullOrEmpty(token))
                    return ExtractEmailFromToken(token);
                return "";
            });
        }
    }
}
