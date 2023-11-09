using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using apiBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace apiBackend.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{

    private readonly PruebaDvpContext _DBContext;

    public IConfiguration _configuration;

    public UserController(PruebaDvpContext dBContext, IConfiguration configuration)
    {
        _DBContext = dBContext;
        _configuration = configuration;
    }

    [HttpGet]
    [Route("getAll")]
    public dynamic GetAll()
    {
        var _users = _DBContext.Users.ToList();

       return new
            {
                success = true,
                message = "successful listing",
                result = _users
            };
    }

    [HttpPost]
    [Route("create")]
    public dynamic Create([FromBody] User _user)
    {
        var user = _DBContext.Users.FirstOrDefault(o => o.Username == _user.Username);

        if (user != null)
        {
            return new
            {
                success = false,
                message = "User already exists",
                result = ""
            };
        }
        else
        {
            _DBContext.Users.Add(_user);
            _DBContext.SaveChanges();
            return new
            {
                success = true,
                message = "user created successfully",
                result = _user
            };
        }

    }

    [HttpPost]
    [Route("auth")]
    public dynamic login([FromBody] object optData)
    {

        var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());

        string username = data.username.ToString();
        string password = data.password.ToString();

        User _user = _DBContext.Users
            .FromSql($"SELECT * FROM [Users] WHERE username = {username} AND password = {password}")
            .FirstOrDefault();

        if (_user == null)
        {
            return new
            {
                success = false,
                message = "Invalid Credentials",
                result = ""
            };
        }

        var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim("username", username),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.key));
        var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            jwt.Issuer,
            jwt.Audience,
            claims,
            expires : DateTime.Now.AddMinutes(60),
            signingCredentials : singIn
        );

         return new
            {
                success = true,
                message = "successful login",
                result = new JwtSecurityTokenHandler().WriteToken(token)
            };
    }
}
