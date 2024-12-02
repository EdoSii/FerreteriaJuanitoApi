using FerreteriaJuanitoApi.Data;
using FerreteriaJuanitoApi.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FerreteriaJuanitoApi.Services
{
    public interface IUserService
    {
        void CreateUser(Usuario usuario);
        bool FindUser(Usuario usuario);
        Usuario GetUser(Login usuario);
        Usuario GetUserById(Guid id);  // Método para obtener un usuario por ID
        void UpdateUser(Usuario usuario);  // Método para actualizar el usuario
        void DeleteUser(Guid id);  // Método para eliminar un usuario

    }
    public class UserService : IUserService
    {
        public readonly DataContext _context;
        public UserService(DataContext dataContext) 
        {
            _context = dataContext;
        }
        // Método para crear usuario
        public void CreateUser(Usuario usuario)
        {

            try
            {
                usuario.MemberSince = DateTime.UtcNow;
                _context.Users.Add(usuario);
                _context.SaveChanges();
            }
            catch(Exception ex) 
            {
                Log.Error(ex, "Hubo un error al tratar de crear usuario");
                throw ex;
            }
        }
        // Método para obtener un usuario por email
        public bool FindUser(Usuario usuario)
        {
            bool existe = false;
            if(_context.Users.Where(u => u.Email == usuario.Email).FirstOrDefault() != null)
            {
                existe = true;
            }
            return existe;
        }
        // Método para obtener un usuario por email y contraseña
        public Usuario GetUser(Login usuario)
        {
            var userAvailable = _context.Users
                .Where(u => u.Email == usuario.Email && u.Pwd == usuario.Pwd)
                .FirstOrDefault();

            if (userAvailable != null)
            {
                return userAvailable;
            }

            Log.Error("Usuario no encontrado");
            throw new Exception("Usuario no encontrado");
        }

        // Método para obtener un usuario por ID
        public Usuario GetUserById(Guid id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        // Método para actualizar un usuario
        public void UpdateUser(Usuario usuario)
        {
            try
            {
                // Busca el usuario a actualizar
                var existingUser = _context.Users.FirstOrDefault(u => u.Id == usuario.Id);
                if (existingUser != null)
                {
                    // Actualiza solo los campos que quieras modificar
                    existingUser.Nombre = usuario.Nombre;
                    existingUser.Apellido = usuario.Apellido;
                    existingUser.Email = usuario.Email;
                    existingUser.Telefono = usuario.Telefono;
                    existingUser.Genero = usuario.Genero;
                    existingUser.Rol = usuario.Rol;

                    // Guarda los cambios
                    _context.Users.Update(existingUser);
                    _context.SaveChanges();
                }
                else
                {
                    Log.Error($"Usuario con id {usuario.Id} no encontrado para actualizar");
                    throw new Exception("Usuario no encontrado");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Hubo un error al intentar actualizar el usuario");
                throw ex;
            }
        }

        // Método para eliminar un usuario
        public void DeleteUser(Guid id)
        {
            try
            {
                // Busca el usuario a eliminar
                var userToDelete = _context.Users.FirstOrDefault(u => u.Id == id);
                if (userToDelete != null)
                {
                    // Elimina el usuario
                    _context.Users.Remove(userToDelete);
                    _context.SaveChanges();
                }
                else
                {
                    Log.Error($"Usuario con id {id} no encontrado para eliminar");
                    throw new Exception("Usuario no encontrado");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Hubo un error al intentar eliminar el usuario");
                throw ex;
            }
        }
        }
}
