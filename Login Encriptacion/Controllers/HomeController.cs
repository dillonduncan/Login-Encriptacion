using System.Diagnostics;
using Login_Encriptacion.Data;
using Login_Encriptacion.Models;
using Login_Encriptacion.Models.ViewModels;
using Login_Encriptacion.Services;
using Microsoft.AspNetCore.Mvc;

namespace Login_Encriptacion.Controllers
{
    public class HomeController : Controller
    {
        private readonly EncriptacionDbContext _dbContext;
        private readonly CryptoService _cryptoS;

        public HomeController(EncriptacionDbContext dbContext, CryptoService cryptoS)
        {
            _cryptoS = cryptoS;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string user, string password)
        {
            var u = _dbContext.Usuarios.FirstOrDefault(u => u.NombreUsuario == user);
            if (u != null && u.PasswordHash == _cryptoS.HashPassword(password) { 
                return RedirectToAction("Formulario");
            }
            ViewBag.Error = "Contraseña o Usuario incorrecto";
            return View(nameof(Index));
        }
        public IActionResult Formulario() => View();

        [HttpPost]
        public IActionResult Procesar(string mensaje)
        {
            var model = new ViewModelResult
            {
                Original = mensaje,
                SimetricoCifrado = _cryptoS.EncriptarSimetrico(mensaje),
                AsimetricoCifrado = _cryptoS.EncriptarAsimetrico(mensaje)
            };
            return View("Resultado", model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
