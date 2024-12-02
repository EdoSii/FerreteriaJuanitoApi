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
            // Validar si el usuario ya existe
            if (_userService.FindUser(usuario))
            {
                Log.Information("Se intentó crear un usuario que ya existe");
                return Ok("Usuario ya existe");
            }
            _userService.CreateUser(usuario);
            Log.Information($"Se creó un usuario nuevo con id: {usuario.Id}");

            return Ok("Usuario creado");
        }

        [AllowAnonymous]
        [HttpPost("LoginUser")]
        public IActionResult Login(Login usuario)
        {
            var userAvailable = _userService.GetUser(usuario);
            // Validar si el usuario está habilitado
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

        // Método para editar usuario
        [Authorize(Roles = "Admin")] // Asegúrate de que solo usuarios con rol Admin puedan editar
        [HttpPut("EditUser/{id}")]
        public IActionResult Edit(Guid id, [FromBody] Usuario usuario)
        {
            var existingUser = _userService.GetUserById(id);

            if (existingUser == null)
            {
                Log.Information($"Usuario con id {id} no encontrado para editar");
                return NotFound("Usuario no encontrado");
            }

            // Actualiza las propiedades del usuario
            existingUser.Nombre = usuario.Nombre;
            existingUser.Apellido = usuario.Apellido;
            existingUser.Email = usuario.Email;
            existingUser.Telefono = usuario.Telefono;
            existingUser.Genero = usuario.Genero;
            existingUser.Rol = usuario.Rol;

            _userService.UpdateUser(existingUser);
            Log.Information($"Usuario con id {id} ha sido actualizado");

            return Ok("Usuario actualizado");
        }

        // Método para eliminar usuario
        [Authorize(Roles = "Admin")] // Solo usuarios con rol Admin pueden eliminar
        [HttpDelete("DeleteUser/{id}")]
        public IActionResult Delete(Guid id)
        {
            var userToDelete = _userService.GetUserById(id);

            if (userToDelete == null)
            {
                Log.Information($"Usuario con id {id} no encontrado para eliminar");
                return NotFound("Usuario no encontrado");
            }

            _userService.DeleteUser(id);
            Log.Information($"Usuario con id {id} ha sido eliminado");

            return Ok("Usuario eliminado");
        }
    }
}
