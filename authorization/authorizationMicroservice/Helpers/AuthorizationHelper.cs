using authorizationMicroservice.Data;
using authorizationMicroservice.Entities;
using authorizationMicroservice.Models;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace authorizationMicroservice.Helpers
{
    public class AuthorizationHelper : IAuthorizationHelper
    {
        public AuthorizationHelper(IMapper mapper,IConfiguration configuration,IFakeUserRepository userRepository)
        {
            Client = new HttpClient() ;
            Mapper = mapper;
            Configuration = configuration;
            UserRepository = userRepository;
            Client.DefaultRequestHeaders.Add("CommunicationKey", Configuration["CommunicationKey:Key"]);
        }

        public HttpClient Client { get; }
        public IMapper Mapper { get; }
        public IConfiguration Configuration { get; }
        public IFakeUserRepository UserRepository { get; }

        public string GenerateJWT(Principal principal,string type)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(Configuration["Jwt:Issuer"],
                                              principal.Username,
                                             new[] { new Claim("Role", type) },
                                             expires: DateTime.Now.AddMinutes(120),
                                             signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User> GetUser(Principal principal,string type)
        {
            /*Kod za pozivanje user mikroservisa
              
              try
            {
                 HttpResponseMessage response = await Client.GetAsync($"{Configuration["ServicesUrl:User"]}?type={type}&username={principal.Username}");
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        return Mapper.Map<User>(JsonSerializer.Deserialize<UserDto>(await response.Content.ReadAsStringAsync()));
                }
                return null;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"error {ex.StatusCode}");
                return null;
            }*/
            User user;
            if(type=="user")
            {
                 user = FakeUserRepository.users.FirstOrDefault(user => user.Username == principal.Username);
            }
            else
            {
                user = FakeUserRepository.admins.FirstOrDefault(user => user.Username == principal.Username);
            }
            return user;
        }

        public bool ValidatePrincipal(Principal principal,string type)
        {
            User user = GetUser(principal,type).Result;

            if (user == null)
                return false;
            if(VerifyPassword(principal.Password,user.Password,user.Salt))
                return true;
            return false;
        }
        public bool VerifyPassword(string password, string hash, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes);
            if (Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256)) == hash)
            {
                return true;
            }
            return false;
        }
    }
}
