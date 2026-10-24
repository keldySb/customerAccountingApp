using Microsoft.AspNetCore.Mvc;
using customerAccountingAppServer.Classes;
using customerAccountingAppServer.Classes.databaseTables;

namespace customerAccountingAppServer.Controllers
{
    [ApiController]
    [Route("api/authorization")]
    public class AuthorizationController : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Login) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new
                {
                    message = "Введите логин и пароль."
                });
            }

            AuthorizationClass authorization = new AuthorizationClass();

            DispatchersTable? user = await authorization.CheckUsers(
                request.Login,
                request.Password
            );

            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "Неверный логин или пароль."
                });
            }

            return Ok(user);
        }
    }

    public class LoginRequest
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
