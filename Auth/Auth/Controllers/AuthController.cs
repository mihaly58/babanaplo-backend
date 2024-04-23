using Auth.Model.Dto;
using Auth.Service.IAuth;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Cors;




namespace Auth.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Microsoft.AspNetCore.Mvc.Route("auth")]
    [Microsoft.AspNetCore.Mvc.ApiController]
    public class AuthController : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [Microsoft.AspNetCore.Mvc.HttpPost("register")]
        public async Task<Microsoft.AspNetCore.Mvc.ActionResult> Register(RegisterRequestDto registerRequestDto)
        {
            var errorMessage = await authService.Register(registerRequestDto);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return StatusCode(400, errorMessage);
            }
            return StatusCode(201, "Sikeres regisztráció");
        }

        //[Authorize]
        [Microsoft.AspNetCore.Mvc.HttpPost("AssignRole")]
        public async Task<Microsoft.AspNetCore.Mvc.ActionResult> AssignRole([FromBody] RoleDto model)
        {

            var assignRoleSuccesful = await authService.AssignRole(model.Email, model.Role.ToUpper());

            if (!assignRoleSuccesful)
            {
                return BadRequest();
            }


            return StatusCode(200, "Sikeres szerep létrehozás.");
        }



        [Microsoft.AspNetCore.Mvc.HttpPost("login")]
        public async Task<Microsoft.AspNetCore.Mvc.ActionResult> Login([Microsoft.AspNetCore.Mvc.FromBody] LoginRequestDto model)
        {
            var loginResponse = await authService.Login(model);

            if (loginResponse.Token == null)
            {
                return BadRequest("Nem megfelelő username vagy jelszó!");
            }

            return StatusCode(200, loginResponse);

        }
    }
}
