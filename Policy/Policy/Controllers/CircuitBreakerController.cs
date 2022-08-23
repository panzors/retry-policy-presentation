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
        private readonly IRouter _router;

        public CircuitBreakerController(CircuitBreakerConnection connection, IRouter router)
        {
            _connection = connection;
            _router = router;
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

        [HttpGet]
        [Route("router")]
        public Task<Receipt> Route()
        {
            return _router.Pay();
        }

        [HttpGet]
        [Route("router-fifty-fity")]
        public Task<Receipt> RouteFiftyFifty()
        {
            return _router.Pay(0.5);
        }
    }
}
