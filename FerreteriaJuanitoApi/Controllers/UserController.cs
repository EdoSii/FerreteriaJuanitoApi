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

        public UserController(IConfiguration configuration, IUserService userService)
        {
            _config = configuration;
            _userService = userService;
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
            var userAvalible = _userService.GetUser(usuario);
            //validar si usuario esta habilitado
            if(userAvalible != null) 
            {
                Log.Information($"Login exitoso para el usuario {usuario.Email}");

                return Ok(new JwtService(_config).GenerateToken(
                    userAvalible.Id.ToString(),
                    userAvalible.Nombre,
                    userAvalible.Apellido,
                    userAvalible.Email,
                    userAvalible.Telefono,
                    userAvalible.Genero
                    )
                );
            }
            Log.Information($"Login rechazado para el usuario {usuario.Email}");

            return Ok("No se pudo hacer login");
        }
    }
}
