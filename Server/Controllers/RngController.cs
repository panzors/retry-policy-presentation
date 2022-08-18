using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RngController
    {
        public static Random r = new Random();

        [HttpGet]
        [Route("safe")]
        public Task<bool> Safe(double failureChance = 0.5)
        {
            if (failureChance < r.NextDouble())
            {
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        [HttpGet]
        [Route("unsafe")]
        public Task<bool> Unsafe(double failureChance = 0.5)
        {
            if (failureChance < r.NextDouble())
            {
                // success
                return Task.FromResult(true);
            }

            throw new Exception();
        }
    }
}
