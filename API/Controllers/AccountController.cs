using System;
using System.Threading.Tasks;
using API.Dto;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : BaseApIController
    {
        private readonly UserManager<AppUser> _usermanager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenRepository _token;
        private readonly IMapper _mapper;
        public AccountController(UserManager<AppUser> usermanager, SignInManager<AppUser> signInManager, ITokenRepository token, IMapper mapper)
        {
            _mapper = mapper;
            _token = token;
            _signInManager = signInManager;
            _usermanager = usermanager;

        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _usermanager.FindByEmailFromClaimsPrinciple(HttpContext.User);
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _token.CreateToken(user, DateTime.Now.AddDays(90))
            };

        }

        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _usermanager.FindByEmailAsync(email) != null;
        }

        [Authorize]
        [HttpGet("addressHistory")]
        public async Task<ActionResult<AddressHistoryDto>> GetUserAddress()
        {
            var user = await _usermanager.FindByUserByClaimsPrincipleWithAddressAsync(HttpContext.User);
            return _mapper.Map<AddressHistoryDto>(user.AddressHistory);
        }

        [Authorize]
        [HttpPut("addressHistory")]
        public async Task<ActionResult<AddressHistoryDto>> UpdateUserAddress(AddressHistoryDto addressHistoryDto)
        {
            var user = await _usermanager.FindByUserByClaimsPrincipleWithAddressAsync(HttpContext.User);
            user.AddressHistory = _mapper.Map<AddressHistory>(addressHistoryDto);
            var result = await _usermanager.UpdateAsync(user);
            if (result.Succeeded) return Ok(_mapper.Map<AddressHistoryDto>(user.AddressHistory));

            return BadRequest("Problem updating user");

        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto login)
        {
            var user = await _usermanager.FindByEmailAsync(login.Email);
            if (user == null) return Unauthorized("Email not found");
            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
            if (!result.Succeeded) return Unauthorized("Invalid Password");
            var userRoles = await _usermanager.GetRolesAsync(user);
            var expToken = DateTime.Now.AddDays(90);
            var refreshToken = _token.GenerateReffreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = expToken;
            await _usermanager.UpdateAsync(user);
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = await _token.CreateToken(user, expToken),
                Email = user.Email,
                Roles = userRoles,
                RefreshTokenExpiry = expToken,
                RefreshToken = refreshToken
            };
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
            {
                return BadRequest("Email address already in used");
            }
            var user = new AppUser
            {
                UserName = registerDto.Email,
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email
            };
            var result = await _usermanager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest("Failed creating user");
            var roleAddResult = await _usermanager.AddToRoleAsync(user, "Member");
            if (!roleAddResult.Succeeded) return BadRequest("Failed to adding role");
            var userRoles = await _usermanager.GetRolesAsync(user);
            var expToken = DateTime.Now.AddDays(90);
            var refreshToken = _token.GenerateReffreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = expToken;
            await _usermanager.UpdateAsync(user);
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _token.CreateToken(user, expToken),
                RefreshToken = refreshToken,
                RefreshTokenExpiry = expToken,
                Roles = userRoles
            };
        }
        [HttpGet("validateRefreshToken")]
        public async Task<ActionResult<UserDto>> ValidateRefreshToken(string email, string rfshToken)
        {
            var user = await _usermanager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new ApiResponse(404, "User not found"));
            }
            var test = user.RefreshToken == rfshToken;
            if (user.RefreshToken == rfshToken && user.RefreshTokenExpiry > DateTime.Now)
            {
                var expToken = DateTime.Now.AddDays(90);
                var refreshToken = _token.GenerateReffreshToken();
                var userRoles = await _usermanager.GetRolesAsync(user);
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiry = expToken;
                await _usermanager.UpdateAsync(user);
                return new UserDto
                {
                    DisplayName = user.DisplayName,
                    Token = await _token.CreateToken(user, expToken),
                    Email = user.Email,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiry = expToken,
                    Roles = userRoles
                };
            }
            return BadRequest(new ApiResponse(400, "Invalid refresh token"));
        }
    }
}