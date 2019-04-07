using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FullStackTeste.WebAPI.Models;

namespace FullStackTest.Web.Controllers
{
    public class LandingController : Controller
    {
        // GET: Landing
        
        public IActionResult Index()
        {
            return View();
        }
    }
}