﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Shared;
using System.Security.Claims;
using System.Text;
using DB.DBO;

namespace Services.Utils {
    internal class TokenGenerator {
        private Config Config { get; }

        public TokenGenerator(Config config) {
            Config = config;
        }

        internal string UserToken(UserDBO user) {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan iat = DateTime.UtcNow - origin;
            int intIAT = (int)Math.Floor(iat.TotalSeconds);

            TimeSpan exp = DateTime.UtcNow.AddDays(Config.JWT.DaysValid) - origin;
            int intEXP = (int)Math.Floor(exp.TotalSeconds);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.ID.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, intIAT.ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, intEXP.ToString())
            };
            var jwtToken = new JwtSecurityToken(Config.JWT.Issuer,
                Config.JWT.Audience ?? "", claims,
                signingCredentials:
                    new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.JWT.SecretKey)),
                        SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}