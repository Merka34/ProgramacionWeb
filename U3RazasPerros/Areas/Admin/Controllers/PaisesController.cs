using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using U3RazasPerros.Models;

namespace U3RazasPerros.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PaisesController : Controller
    {
        public PaisesController(perrosContext context)
        {
            Context = context;
        }

        public perrosContext Context { get; }

        public IActionResult Index()
        {
            IEnumerable<Paises> paises = Context.Paises.OrderBy(x=>x.Nombre);
            return View(paises);
        }

        public IActionResult Agregar()
        {
            return View(new Paises());
        }

        [HttpPost]
        public IActionResult Agregar(Paises p)
        {
            if (string.IsNullOrWhiteSpace(p.Nombre))
            {
                ModelState.AddModelError("", "Agregue un nombre al pais");
                return View(p);
            }
            if (Context.Paises.Any(x=>x.Nombre==p.Nombre))
            {
                ModelState.AddModelError("", "Ya existe un pais con ese nombre");
                return View(p);
            }
            Context.Paises.Add(p);
            Context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Editar(int id)
        {
            Paises pais = Context.Paises.FirstOrDefault(x=>x.Id==id);
            if (pais==null)
            {
                return RedirectToAction("Index");
            }
            return View(pais);
        }

        [HttpPost]
        public IActionResult Editar(Paises p)
        {
            if (string.IsNullOrWhiteSpace(p.Nombre))
            {
                ModelState.AddModelError("", "Agregue un nombre al pais");
                return View(p);
            }
            if (Context.Paises.Any(x => x.Nombre == p.Nombre))
            {
                ModelState.AddModelError("", "Ya existe un pais con ese nombre");
                return View(p);
            }
            Paises pais = Context.Paises.FirstOrDefault(x=>x.Id == p.Id);
            pais.Nombre = p.Nombre;
            Context.Paises.Update(pais);
            Context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Eliminar(int id)
        {
            Paises p = Context.Paises.FirstOrDefault(x=>x.Id == id);
            if (p == null)
            {
                return RedirectToAction("Index");
            }
            return View(p);
        }

        [HttpPost]
        public IActionResult Eliminar(Paises p)
        {
            Paises pais = Context.Paises.FirstOrDefault(x=>x.Id == p.Id);
            if (pais==null)
            {
                ModelState.AddModelError("", "No se encontro el pais, puede que no exista o ya haya sido eliminado");
                return View(pais);
            }
            if (Context.Razas.Any(x=>x.IdPais==p.Id))
            {
                ModelState.AddModelError("", "No se puede eliminar este pais porque hay razas registrados con este pais");
                return View(pais);
            }
            Context.Paises.Remove(pais);
            Context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
