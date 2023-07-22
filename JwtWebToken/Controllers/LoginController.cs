using JwtWebToken.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace JwtWebToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        public LoginController(IConfiguration configuration)
        {
            _config= configuration;
        }

        private Users AuthenticateUser(Users user)
        {
            Users _user = null;
            if(user.Username=="admin" && user.Password=="123")
            {
                _user = new Users
                {
                    Username = "Taosif",
                };
            }
            return _user;
        }

        private string GenerateToken(Users users)
        {
            var securityToken = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]));
            var credentials= new SigningCredentials(securityToken,SecurityAlgorithms.HmacSha256);

            var Token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], null,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials:credentials
                ); 
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(Users user)
        {
            IActionResult response= Unauthorized();
            var user_= new Users();
            if(user!=null)
            {
                var Token=GenerateToken(user_);
                response=Ok( new {Token= Token});
            }
            return response;
        }
    }
}
