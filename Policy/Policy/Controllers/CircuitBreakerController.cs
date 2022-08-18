using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WantACracker.Models;
using WantACracker.Services;

namespace WantACracker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CircuitBreakerController
    {
        private CircuitBreakerConnection _connection;

        public CircuitBreakerController(CircuitBreakerConnection connection)
        {
            _connection = connection;
        }

        [HttpGet]
        [Route("simple")]
        public Task<Cracker> Simple()
        {
            return _connection.Successful();
        }

        [HttpGet]
        [Route("fifty-fifty")]
        public Task<Cracker> FiftyFifty()
        {
            return _connection.FiftyFiftyUnsafe();
        }

        [HttpGet]
        [Route("fail")]
        public Task<Cracker> Fail()
        {
            return _connection.Fail();
        }
    }
}
