using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FullStackTeste.API.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FullStackTeste.API.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // POST api/values
        [HttpPost]
        [EnableCors("MyPolicy")]
        public void Post(string value)
        {
            var estatisticas = JsonConvert.DeserializeObject<Statistics>(value);

        }
    }
}
