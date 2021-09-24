using Microsoft.AspNetCore.Mvc;
using ProWebU1Act2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProWebU1Act2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(IndexViewModel vm)
        {
            return View(vm);
        }
    }
}
