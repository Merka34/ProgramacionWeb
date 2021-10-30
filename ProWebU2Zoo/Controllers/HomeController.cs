using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProWebU2Zoo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProWebU2Zoo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            animalesContext context = new animalesContext();
            var datos = context.Clases.Include(x => x.Especies).OrderBy(x=>x.Nombre);
            return View(datos);
        }

        [Route("Especies-{nombre}")]
        public IActionResult Especies(string nombre)
        {
            string _nombre = nombre.Replace("-", " ");
            animalesContext context = new animalesContext();
            IEnumerable<Clase> especies = context.Clases.Include(x => x.Especies).ThenInclude(x => x.IdClaseNavigation).Where(x => x.Nombre == _nombre);
            if (especies.Count()==0)
            {
                return RedirectToAction("Index");
            }
            return View(especies);
        }

        [Route("Especie/{especie}")]
        public IActionResult Especie(string especie)
        {
            string _especie = especie.Replace("-", " ");
            animalesContext context = new animalesContext();
            var es = context.Especies.Include(x=>x.IdClaseNavigation).FirstOrDefault(x=>x.Especie==_especie);
            if (es==null)
            {
                return RedirectToAction("Index");
            }
            return View(es);
        }
    }
}
