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

    }
    public class UserService : IUserService
    {
        public readonly DataContext _context;
        public UserService(DataContext dataContext) 
        {
            _context = dataContext;
        }
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
        public bool FindUser(Usuario usuario)
        {
            bool existe = false;
            if(_context.Users.Where(u => u.Email == usuario.Email).FirstOrDefault() != null)
            {
                existe = true;
            }
            return existe;
        }

        public Usuario GetUser(Login usuario)
        {
            var userAvailible = _context.Users.Where(u => u.Email == usuario.Email && u.Pwd == usuario.Pwd).FirstOrDefault();

            if(userAvailible != null) { return userAvailible; }

            Log.Error("Usuario no encontrado");
            throw new Exception();
        }
    }

}
