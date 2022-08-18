using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FailController : ControllerBase
    {
        [HttpGet]
        public Task<IActionResult> Get()
        {
            throw new Exception();
        }
    }
}
