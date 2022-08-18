using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WantACracker.Models;
using WantACracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WantACracker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NormalController
    {
        private readonly NormalConnection _server;

        public NormalController(NormalConnection server)
        {
            _server = server;
        }

        [HttpGet]
        [Route("simple")]
        public Task<Cracker> Simple() 
        {
            return _server.Successful();
        }

        [HttpGet]
        [Route("fifty-fifty")]
        public Task<Cracker> FiftyFifty()
        {
            return _server.FiftyFiftyUnsafe();
        }

        [HttpGet]
        [Route("sleep")]
        public Task<Cracker> Sleep(int durationMs = 1000)
        {
            return _server.Sleep(durationMs);
        }
    }
}
