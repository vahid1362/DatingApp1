using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp1.Data;
using DatingApp1.Model;
using DatingApp1.Model.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
         
            return Ok("");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userName = model.UserName.ToLower();

            if (await _repo.UserExist(userName))
            {
                return BadRequest("Invalid  username");
            }
            var userToCreate = new User()
            {
                UserName = userName
            };
            _repo.Register(userToCreate, model.Password);
            return StatusCode(201);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            var user = await _repo.Login(login.UserName, login.Password);

            if (user == null)
                return Unauthorized();
            var claimes = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                 new  Claim(ClaimTypes.Name, user.UserName)

            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescripto = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claimes),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescripto);
            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}