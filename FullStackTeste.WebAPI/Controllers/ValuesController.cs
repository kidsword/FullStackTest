using FullStackTeste.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FullStackTeste.WebAPI
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [HttpPost]
        public void Post([FromBody]Statistics value)
        {
            var estatisticas =  JsonConvert.SerializeObject(value);

            RabbitMQ.Helper.Sender
                .Create("localhost", "statistic")
                .SendMessage(estatisticas);

        }

    }
}
