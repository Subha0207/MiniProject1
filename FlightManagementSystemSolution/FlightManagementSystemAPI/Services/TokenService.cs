﻿using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FlightManagementSystemAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _secretKey;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration configuration)
        {
            // initial value of key from json file 
            _secretKey = configuration.GetSection("TokenKey").GetSection("JWT").Value.ToString();
            // encrypt the key 
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        }
        public string GenerateToken(User user)
        {
            string token = string.Empty;
            // What all data we want to pass in its body 
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserId.ToString()),
                new Claim(ClaimTypes.Role,user.Role),
                new Claim(ClaimTypes.Email,user.Email)
            };
            //select algo and key for signing 
            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            // get the token with owner , user, body , expiry , and the credentials (5 parameters)
            var myToken = new JwtSecurityToken(null, null, claims, expires: DateTime.Now.AddDays(2), signingCredentials: credentials);
            token = new JwtSecurityTokenHandler().WriteToken(myToken);
            return token;
        }
    }
}
