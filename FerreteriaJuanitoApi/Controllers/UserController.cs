using FerreteriaJuanitoApi.Data;
using FerreteriaJuanitoApi.Models;
using FerreteriaJuanitoApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FerreteriaJuanitoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public UserController(IConfiguration configuration, IUserService userService, IJwtService jwtService)
        {
            _config = configuration;
            _userService = userService;
            _jwtService = jwtService;
        }

        [AllowAnonymous]
        [HttpPost("CreateUser")]
        public IActionResult Create(Usuario usuario)
        {
            //validar si usuario existe
            if(_userService.FindUser(usuario)) 
            {
                Log.Information("Se intento crear un usuario que ya existe");

                return Ok("Usuario ya existe");
            }
            _userService.CreateUser(usuario);
            Log.Information($"Se creo un usuario nuevo con id: {usuario.Id}");

            return Ok("Usuario Creado");
        }

        [AllowAnonymous]
        [HttpPost("LoginUser")]
        public IActionResult Login(Login usuario)
        {
            var userAvailable = _userService.GetUser(usuario);
            // validar si el usuario está habilitado
            if (userAvailable != null)
            {
                Log.Information($"Login exitoso para el usuario {usuario.Email}");

                return Ok(_jwtService.GenerateToken(
                    userAvailable.Id.ToString(),
                    userAvailable.Nombre,
                    userAvailable.Apellido,
                    userAvailable.Email,
                    userAvailable.Telefono,
                    userAvailable.Genero,
                    userAvailable.Rol
                ));
            }
            Log.Information($"Login rechazado para el usuario {usuario.Email}");

            return Ok("No se pudo hacer login");
        }
    }
}
