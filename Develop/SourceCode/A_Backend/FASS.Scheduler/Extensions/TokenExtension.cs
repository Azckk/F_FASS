using Common.NETCore.Helpers;
using FASS.Scheduler.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace FASS.Scheduler.Extensions
{
    public static class TokenExtension
    {
        public static string GetToken(this AppSettings appSettings, IEnumerable<Claim> claims)
        {
            var signingKey = appSettings.Auth.SigningKey;
            var algorithm = SecurityAlgorithms.HmacSha256;
            var issuer = appSettings.Auth.Issuer;
            var audience = appSettings.Auth.Audience;
            var notBefore = DateTime.Now;
            var expires = notBefore.AddSeconds(appSettings.Auth.ExpireSeconds);
            var token = JwtHelper.CreateToken(signingKey, algorithm, issuer, audience, claims, notBefore, expires);
            return token;
        }

        public static string RefreshToken(this AppSettings appSettings, string token)
        {
            var signingKey = appSettings.Auth.SigningKey;
            var refreshToken = JwtHelper.RefreshToken(token, signingKey);
            return refreshToken;
        }
    }
}