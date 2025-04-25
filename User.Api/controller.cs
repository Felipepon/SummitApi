using Microsoft.AspNetCore.Mvc;
using User.Api.Core;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace User.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetUser")]
        public IActionResult GetUsers([FromQuery] string username = null)
        {
            var json = GetUserBusiness.Process(username);
            return Ok(json);
        }

        [HttpPost("SaveUser")]
        public async Task<IActionResult> SaveUser()
        {
            var result = await SaveUserBusiness.Process();
            return result == ServiceState.Accepted ? Ok(result) : StatusCode(500, "Error saving user.");
        }

       [HttpPost("ValidateUser")]
public IActionResult ValidateUser([FromBody] LoginRequest user)
{
    var userJson = Database.Instance.GetJson(user.UserName);
    Console.WriteLine($"User JSON: {userJson}");

    if (string.IsNullOrEmpty(userJson) || userJson == "{}")
    {
        return NotFound("User not found.");
    }

    var deserializedUser = JsonSerializer.Deserialize<User.Api.Core.User>(userJson);
    Console.WriteLine($"Deserialized User: {JsonSerializer.Serialize(deserializedUser)}");

    var result = PasswordHash.Validate(user.Password, user.UserName, deserializedUser.Password);
    return result == ServiceState.Accepted ? Ok(result) : StatusCode(403, "Credentials are invalid.");
}
    }

    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}