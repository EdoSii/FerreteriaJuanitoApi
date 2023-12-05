using FerreteriaJuanitoApi.Data;
using FerreteriaJuanitoApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FerreteriaJuanitoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarritoController : ControllerBase
    {
        private readonly ICarritoService _carritoService;
        public readonly DataContext _context;
        public CarritoController(ICarritoService carritoService, DataContext context)
        {
            _carritoService = carritoService;
            _context = context;
        }
    }
}
