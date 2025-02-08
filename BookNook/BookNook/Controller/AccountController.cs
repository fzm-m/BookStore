using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using BookNook.Data;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using static BookNook.Client.Pages.Account.Register;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BookNook.Domain;
using BookNook.Client.Models;
using BookNook.Client.Pages.User.UserHome;

namespace BookNook.Controller
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<BookNookUser> _userManager; 
        private readonly BookNookDbContext _DbContext;
        private readonly IConfiguration _configuration;
        public AccountController(UserManager<BookNookUser> userManager, BookNookDbContext DbContext, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
            _DbContext = DbContext;
        }
        public class LoginModel
        {
            public string? UserName { get; set; }
            public string? Password { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? Email { get; set; }
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginModel loginModel)
        {
            try
            {
                BookNookUser user = await _userManager.FindByNameAsync(loginModel.UserName);
                if (user == null)
                {
                    user = new BookNookUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = loginModel.UserName,
                        FirstName = loginModel.FirstName,
                        LastName = loginModel.LastName,
                        Email = loginModel.Email,
                        EmailConfirmed = true,
                        NormalizedEmail = loginModel.Email.ToUpper(),
                        NormalizedUserName = loginModel.UserName.ToUpper(),
                    };
                    var result = await _userManager.CreateAsync(user, loginModel.Password);

                    if (result.Succeeded)
                    {
                        var r = await _userManager.AddToRoleAsync(user, "USER");
                        return Ok("User registration successful");
                    }
                    return BadRequest(result.Errors);
                }
                else
                {
                    return BadRequest("The user already exists");
                }
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
            
        }
     
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                string userName = model.UserName;
                string password = model.Password;
                var user = await _userManager.FindByNameAsync(userName);
                if (user == null)
                {
                    return NotFound("user name does not exist");
                }
                var success = await _userManager.CheckPasswordAsync(user, password);

                if (success)
                {

                    var token = GenerateJwtToken(user);
                    List<string> roles = (List<string>)await _userManager.GetRolesAsync(user);
                    string role = roles.FirstOrDefault();
                    return Ok(new { Token = token, userId = user.Id, userName = userName, role = role });
                }
                return Unauthorized("Login failed");
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
            
        }
        private string GenerateJwtToken(BookNookUser user)
        {
            try
            {
                var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex) {
                return "";
            }
            
        }

       
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser(string userId)
        {
            try
            {
                BookNookUser bookNookUser = await _userManager.FindByIdAsync(userId);
                User user = new User();
                user.UserId = userId;
                user.UserName = bookNookUser.UserName;
                user.FirstName = bookNookUser.FirstName;
                user.LastName = bookNookUser.LastName;
                user.Email = bookNookUser.Email;


                Domain.Membership membership = _DbContext.Memberships.Where(m => m.UserId == userId).FirstOrDefault();
                if (membership != null)
                {
                    Domain.MembershipLevel membershipLevel = _DbContext.MembershipLevels.Where(m => m.Id == membership.MembershipLevelId).FirstOrDefault();
                    user.MembershipLevelName = membershipLevel.MembershipLevelName;
                    user.DiscountRate = membershipLevel.DiscountRate;
                }
                return Ok(user);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
            
        }
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] User user)
        {
            try
            {
                BookNookUser bookNookUser = await _userManager.FindByIdAsync(user.UserId);
                var success = await _userManager.ChangePasswordAsync(bookNookUser, user.OldPassword, user.NewPassword);
                if (success.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return Unauthorized("failed");
                }
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
           
        }


        [HttpGet("getTotalCount")]
        public IActionResult getTotalCount(string? userName, int isMember)
        {
            try
            {
                int totalCount = 0;
                if (!string.IsNullOrEmpty(userName))
                {
                    totalCount = _DbContext.Users.Where(i => i.UserName == userName).Count();
                }
                else
                {
                    if (isMember == 1)
                    {
                        totalCount = _DbContext.Memberships.Count();
                    }
                    else if (isMember == 2)
                    {
                        List<string> idList = _DbContext.Memberships.Select(i => i.UserId).ToList();
                        totalCount = _DbContext.Users.Where(u => !idList.Contains(u.Id)).Count();
                    }
                    else
                    {
                        totalCount = _DbContext.Users.Count();
                    }
                }

                return Ok(totalCount);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
            
        }
        [HttpGet("GetUserList")]
        public IActionResult GetItems(string? userName, int isMember, int page = 1, int pageSize = 5)
        {
            try
            {
                List<User> items = new List<User>();
                List<BookNookUser> bookNookUsers = new List<BookNookUser>();

                if (!string.IsNullOrEmpty(userName))
                {
                    bookNookUsers = _DbContext.Users.Where(i => i.UserName == userName).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                }
                else
                {
                    List<string> idList = _DbContext.Memberships.Select(i => i.UserId).ToList();
                    if (isMember == 1)
                    {
                        bookNookUsers = _DbContext.Users.Where(u => idList.Contains(u.Id)).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    }
                    else if (isMember == 2)
                    {
                        bookNookUsers = _DbContext.Users.Where(u => !idList.Contains(u.Id)).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    }
                    else
                    {
                        bookNookUsers = _DbContext.Users.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    }
                }

                foreach (var bookNookUser in bookNookUsers)
                {
                    User user = new User();
                    user.UserId = bookNookUser.Id;
                    user.UserName = bookNookUser.UserName;
                    user.FirstName = bookNookUser.FirstName;
                    user.LastName = bookNookUser.LastName;
                    user.Email = bookNookUser.Email;

                    Domain.Membership membership = _DbContext.Memberships.Where(m => m.UserId == bookNookUser.Id).FirstOrDefault();
                    if (membership != null)
                    {
                        Domain.MembershipLevel membershipLevel = _DbContext.MembershipLevels.Where(m => m.Id == membership.Id).FirstOrDefault();
                        user.MembershipLevelName = membershipLevel.MembershipLevelName;
                    }
                    items.Add(user);
                }


                return Ok(items);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
            
        }

       

    }
}
