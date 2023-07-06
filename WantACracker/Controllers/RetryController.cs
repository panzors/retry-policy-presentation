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
    public class RetryController
    {
        private RetryConnection _connection;

        public RetryController(RetryConnection server)
        {
            _connection = server;
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
            return _connection.FiftyFiftyUnsafe(0.5);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="failChance">value between 0 and 1. 1 representing 100% chance of failure</param>
        /// <returns></returns>
        [HttpGet]
        [Route("fail-chance")]
        public Task<Cracker> FailChance(double failChance = 0.5)
        {
            return _connection.FiftyFiftyUnsafe(failChance);
        }

        [HttpGet]
        [Route("sleep")]
        public Task<Cracker> Sleep(int sleepms = 1000)
        {
            return _connection.Sleep(sleepms);
        }
    }
}
