using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UnitedRemote.Core.Helpers;
using UnitedRemote.Core.Models.V1;
using UnitedRemote.Core.Repositories.Interfaces;
using UnitedRemote.Core.ViewModels;

namespace UnitedRemote.Web.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUsersRepository _usersRepository;

        private readonly IErrorHandler _errorHandler;
        private readonly IConfiguration _configuration;

        public AccountController(
            IUsersRepository usersRepository,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IErrorHandler errorHandler)
        {
            _usersRepository = usersRepository;
            _signInManager = signInManager;
            _configuration = configuration;
            _errorHandler = errorHandler;
        }

        [HttpPost]
        public async Task<object> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpRequestException(string.Format(
                    _errorHandler.GetMessage(ErrorMessagesEnum.ModelValidation),
                    ModelState.Values.First().Errors.First().ErrorMessage));
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                var user = await _usersRepository.GetByEmailAsync(model.Email);
                return new { fullName = $"{user.FirstName} {user.LastName}", token = GenerateJwtToken(model.Email, user)};
            }

            throw new HttpRequestException(_errorHandler.GetMessage(ErrorMessagesEnum.AuthWrongCredentials));
        }

        [HttpPost]
        public async Task<object> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpRequestException(string.Format(
                    _errorHandler.GetMessage(ErrorMessagesEnum.ModelValidation),
                    ModelState.Values.First().Errors.First().ErrorMessage));
            }

            var user = new ApplicationUser
            {
                FirstName = model.Firstname,
                LastName = model.Lastname,
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.Mobile
            };
            var result = await _usersRepository.Create(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return new { fullName = $"{user.FirstName} {user.LastName}", token = GenerateJwtToken(model.Email, user) };
            }

            if (result.Errors.Count() > 0)
            {
                return BadRequest(result.Errors);
            }

            throw new HttpRequestException(_errorHandler.GetMessage(ErrorMessagesEnum.AuthCannotCreate));
        }

        private object GenerateJwtToken(string email, ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}