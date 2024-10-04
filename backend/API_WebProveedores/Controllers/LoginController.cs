using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebProov_API.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebProov_API.Data.Interfaces;
using WebProov_API.Dtos;
using AutoMapper;
using Microsoft.Office.Interop.Excel;

namespace WebProov_API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ILoginRepository _repo;
        private readonly IConfiguration _config;
        private IMapper _mapper;

        public LoginController(ILoginRepository repo, IConfiguration config, IMapper mapper)
        {
            _config = config;
            _repo = repo;
            _mapper = mapper;
        }

        //[AllowAnonymous]
        [HttpPost]
        public ActionResult<User> Login([FromBody] User userLogin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = Authenticate(userLogin);
            if (user.Flag)
            {
                var token = Generate(user);
                user.Token = token;
                //return Ok(token);
            }
            return Ok(_mapper.Map<User>(user));
            //return NotFound("Fallo Login");
        }


        private string Generate(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credential=new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Code),
                new Claim(ClaimTypes.Name,user.Name),
                new Claim(ClaimTypes.Email,user.Mail),
                new Claim(ClaimTypes.Authentication,user.Ruc),
                new Claim(ClaimTypes.Dns,user.CreditLine)
            };

            var token = new JwtSecurityToken(null,null,
                claims,
                expires:DateTime.UtcNow.AddHours(4),
                signingCredentials:credential);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User Authenticate(User userLogin)
        {
            //var currentUser = UserConstants.Users.FirstOrDefault(o => o.Username.ToLower() ==
            //userLogin.Username.ToLower() && o.Password == userLogin.Password);
            var currentUser = _repo.GetLogin(userLogin);
            if (currentUser != null)
            {
                return currentUser;
            }
            return null;
        }

    }

}
