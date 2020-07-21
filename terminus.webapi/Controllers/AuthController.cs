using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using terminus.shared.models;

namespace terminus.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInMgr;
        private readonly UserManager<AppUser> _userMgr;
        private readonly RoleManager<AppRole> _roleMgr;
        private readonly IPasswordHasher<AppUser> _hasher;
        private readonly IConfiguration _config;

        public AuthController(SignInManager<AppUser> signInMgr,
            UserManager<AppUser> userMgr,
             RoleManager<AppRole> roleMgr,
             IPasswordHasher<AppUser> hasher,
             IConfiguration config
            )
        {
            _signInMgr = signInMgr;
            _userMgr = userMgr;
            _roleMgr = roleMgr;
            _hasher = hasher;
            _config = config;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] LoginViewModel model)
        {
            var user = await _userMgr.FindByNameAsync(model.userName);
            if (user != null)
            {
                if (_hasher.VerifyHashedPassword(user, user.PasswordHash, model.password) ==
                     PasswordVerificationResult.Success)
                {
                    var userClaims = await _userMgr.GetClaimsAsync(user);
                    var claims = new[]
                    {
                                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                new Claim(JwtRegisteredClaimNames.Email, user.Email)
                            }.Union(userClaims);

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: _config["Jwt:issuer"],
                        audience: _config["Jwt:issuedTo"],
                        claims: claims,
                        expires: DateTime.UtcNow.AddMinutes(20),
                        signingCredentials: creds
                    ); ;

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                    return Ok(new 
                    { 
                        token = tokenString,
                        expiration = token.ValidTo
                    });
                }
            }

            return Unauthorized();


        }
    }
}
