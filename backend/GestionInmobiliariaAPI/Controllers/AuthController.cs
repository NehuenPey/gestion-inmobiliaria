using GestionInmobiliariaAPI.DTOs;
using GestionInmobiliariaAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionInmobiliariaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registrar(RegistroDto dto)
        {
            var registrado = await _authService.RegistrarUsuario(dto);

            if (!registrado)
            {
                return Conflict(new
                {
                    mensaje = "El email ya está registrado."
                });
            }

            return StatusCode(201, new
            {
                mensaje = "Usuario registrado correctamente."
            });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var credencialesValidas =
                await _authService.ValidarCredenciales(dto);

            if (!credencialesValidas)
            {
                return Unauthorized(new
                {
                    mensaje = "Email o contraseña incorrectos."
                });
            }

            return Ok(new
            {
                mensaje = "Login correcto."
            });
        }
    }
}