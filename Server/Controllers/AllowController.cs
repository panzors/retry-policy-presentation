using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AllowController : ControllerBase
    {
        [HttpGet]
        public Task<bool> Get()
        {
            return Task.FromResult(true);
        }
    }
}
