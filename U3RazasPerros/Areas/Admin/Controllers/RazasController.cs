using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using U3RazasPerros.Areas.Admin.Models;
using U3RazasPerros.Models;

namespace U3RazasPerros.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RazasController : Controller
    {
        public perrosContext Context { get; }
        public IWebHostEnvironment Host { get; }

        public RazasController(perrosContext context, IWebHostEnvironment host)
        {
            Context = context;
            Host = host;
        }

        public IActionResult Index()
        {
            IEnumerable<Razas> razas = Context.Razas.OrderBy(x=>x.Nombre);
            return View(razas);
        }

        public IActionResult Agregar()
        {
            return View(new RazaCRUDViewModel {
                Paises = Context.Paises.OrderBy(x => x.Nombre)
            });
        }

        [HttpPost]
        public IActionResult Agregar(RazaCRUDViewModel vm, IFormFile imagen)
        {
            if (!Verificacion(vm, out string m))
            {
                ModelState.AddModelError("", m);
                vm.Paises = Context.Paises.OrderBy(x=>x.Nombre);
                return View(vm);
            }
            if (imagen!=null)
            {
                if (imagen.ContentType!="image/jpeg")
                {
                    ModelState.AddModelError("", "Solo se permite la carga de archivos JPG");
                    vm.Paises = Context.Paises.OrderBy(x => x.Nombre);
                    return View(vm);
                }
                if (imagen.Length>1024*1024*5)
                {
                    ModelState.AddModelError("", "No se permite la carga de archivos mayores a 5MB");
                    vm.Paises = Context.Paises.OrderBy(x => x.Nombre);
                    return View(vm);
                }
            }
            Context.Add(vm.Raza);
            Context.SaveChanges();
            //Context.Caracteristicasfisicas.Add(vm.Caracteristicas);
            //Context.SaveChanges();

            if (imagen!=null)
            {
                var path = Host.WebRootPath + "/imgs_perros/" + vm.Raza.Id + "_0.jpg";
                FileStream fs = new FileStream(path, FileMode.Create);
                imagen.CopyTo(fs);
                fs.Close();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Editar(int id)
        {
            Razas raza = Context.Razas.FirstOrDefault(x=>x.Id==id);
            Caracteristicasfisicas c = Context.Caracteristicasfisicas.FirstOrDefault(x=>x.Id==id);
            if (raza==null)
            {
                return RedirectToAction("Index");
            }
            RazaCRUDViewModel vm = new RazaCRUDViewModel
            {
                Raza = raza, Paises = Context.Paises.OrderBy(x=>x.Nombre)
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult Editar(RazaCRUDViewModel vm, IFormFile imagen)
        {
            var raza = Context.Razas.Include(x=>x.Caracteristicasfisicas).FirstOrDefault(x => x.Id == vm.Raza.Id);
            if (raza==null)
            {
                return RedirectToAction("Index");
            }
            if (!Verificacion(vm, out string m))
            {
                ModelState.AddModelError("", m);
                vm.Paises = Context.Paises.OrderBy(x=>x.Nombre);
                return View(vm);
            }
            if (imagen != null)
            {
                if (imagen.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("", "Solo se permite la carga de archivos JPG");
                    vm.Paises = Context.Paises.OrderBy(x => x.Nombre);
                    return View(vm);
                }
                if (imagen.Length > 1024 * 1024 * 5)
                {
                    ModelState.AddModelError("", "No se permite la carga de archivos mayores a 5MB");
                    vm.Paises = Context.Paises.OrderBy(x => x.Nombre);
                    return View(vm);
                }
            }
            raza.Nombre = vm.Raza.Nombre;
            raza.OtrosNombres = vm.Raza.OtrosNombres;
            raza.IdPais = vm.Raza.IdPais;
            raza.PesoMin = vm.Raza.PesoMin;
            raza.PesoMax = vm.Raza.PesoMax;
            raza.AlturaMin = vm.Raza.AlturaMin;
            raza.AlturaMax = vm.Raza.AlturaMax;
            raza.EsperanzaVida = vm.Raza.EsperanzaVida;

            raza.Caracteristicasfisicas.Cola = vm.Raza.Caracteristicasfisicas.Cola;
            raza.Caracteristicasfisicas.Color = vm.Raza.Caracteristicasfisicas.Color;
            raza.Caracteristicasfisicas.Hocico = vm.Raza.Caracteristicasfisicas.Hocico;
            raza.Caracteristicasfisicas.Patas = vm.Raza.Caracteristicasfisicas.Patas;
            raza.Caracteristicasfisicas.Pelo = vm.Raza.Caracteristicasfisicas.Pelo;

            Context.Update(raza);
            Context.SaveChanges();
            if (imagen != null)
            {
                var path = Host.WebRootPath + "/imgs_perros/" + vm.Raza.Id + "_0.jpg";
                FileStream fs = new FileStream(path, FileMode.Create);
                imagen.CopyTo(fs);
                fs.Close();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Eliminar(int id)
        {
            Razas raza = Context.Razas.FirstOrDefault(x=>x.Id==id);
            if (raza==null)
            {
                return RedirectToAction("Index");
            }
            return View(raza);
        }

        [HttpPost]
        public IActionResult Eliminar(Razas vm)
        {
            Razas raza = Context.Razas.FirstOrDefault(x=>x.Id == vm.Id);
            Caracteristicasfisicas caracteristicas = Context.Caracteristicasfisicas.FirstOrDefault(x => x.Id == vm.Id);
            if (raza==null || caracteristicas==null)
            {
                ModelState.AddModelError("", "No se encontro la rza, puede que no exista o ya haya sido eliminado");
                return View(vm);
            }

            Context.Remove(caracteristicas);
            Context.Remove(raza);
            Context.SaveChanges();

            string path = Host.WebRootPath + "/imgs_perros/" + raza.Id + "_0.jpg";
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            return RedirectToAction("Index");
        }

        private bool Verificacion(RazaCRUDViewModel vm, out string mensaje)
        {
            if (string.IsNullOrWhiteSpace(vm.Raza.Nombre))
            {
                mensaje = "Asigne un valor a nombre";
                return false;
            }
            if (string.IsNullOrWhiteSpace(vm.Raza.OtrosNombres))
            {
                vm.Raza.OtrosNombres = "No tiene";
            }
            if (string.IsNullOrWhiteSpace(vm.Raza.Descripcion))
            {
                mensaje = "Asigne una descripcion";
                return false;
            }
            if (vm.Raza.IdPais==0)
            {
                mensaje = "Asigne un pais";
                return false;
            }
            if (vm.Raza.PesoMin==0)
            {
                mensaje = "Asigne el peso mínimo";
                return false;
            }
            if (vm.Raza.PesoMax == 0)
            {
                mensaje = "Asigne el peso máximo";
                return false;
            }
            if (vm.Raza.PesoMin>= vm.Raza.PesoMax)
            {
                mensaje = "El peso minimo debe ser menor al Peso Máximo";
                return false;
            }
            if (vm.Raza.AlturaMin == 0)
            {
                mensaje = "Asigne la altura mínima";
                return false;
            }
            if (vm.Raza.AlturaMax == 0)
            {
                mensaje = "Asigne la altura máxima";
                return false;
            }
            if (vm.Raza.AlturaMin >= vm.Raza.AlturaMax)
            {
                mensaje = "El peso minimo debe ser menor al Peso Máximo";
                return false;
            }
            if (vm.Raza.EsperanzaVida == 0)
            {
                mensaje = "Asigne una esperanza de vida";
                return false;
            }


            mensaje = "";
            return true;
        }
    }
}
