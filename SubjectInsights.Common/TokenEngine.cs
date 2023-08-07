using Jose;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace SubjectInsights.Common
{
    public static class TokenEngine
    {
        private static byte[] _secretKey = Encoding.UTF8.GetBytes("G1212U23I dsfds3L324dsfD 208sjjf");
        private static byte[] _secretResetPasswordKey = Encoding.UTF8.GetBytes("G1212U23I 208sjjf dsfds3L324dsfD");
        public enum TokenType { User, PasswordReset }
        /// <summary>
        /// generateJwtToken
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GenerateJwtToken(Claim[] claims, bool isResetPassword = false)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "http://localhost:3000",
                Audience = "http://localhost:3000",
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(isResetPassword ? 10 : 120),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(isResetPassword ? _secretResetPasswordKey : _secretKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        /// <summary>
        /// 藉由產生的 access token, 取得有限期間 (default 120 minutes) 授權的 auth token
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static string CreateToken<T>(T payload) where T : TokenPayloadBase
        {
            var experiationTime = 120; //120 minutes
            payload.Created = DateTime.UtcNow;
            payload.Expiration = DateTime.UtcNow.AddMinutes(experiationTime);
            return JWT.Encode(payload, _secretKey, JwsAlgorithm.HS256);
        }

        /// <summary>
        /// 取得 auth token 的 payload 內容
        /// </summary>
        /// <param name="tokenText">auth token text, JWT format</param>
        /// <returns></returns>
        public static T DecodeTokenPayload<T>(string tokenText) where T : TokenPayloadBase
        {
            try
            {
                T payload = JWT.Decode<T>(tokenText, _secretKey, JwsAlgorithm.HS256);
                return payload;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static IPrincipal ValidateToken(string authToken, bool isResetPassword = false)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters(isResetPassword);

            SecurityToken validatedToken;
            return tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
        }

        public static TokenValidationParameters GetValidationParameters(bool isResetPassword = false)
        {
            string key = Encoding.UTF8.GetString(isResetPassword ? _secretResetPasswordKey : _secretKey);
            return new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "http://localhost:3000",
                ValidAudience = "http://localhost:3000",
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(isResetPassword ? _secretResetPasswordKey : _secretKey)//make sure key length big
            };
        }
    }
}
