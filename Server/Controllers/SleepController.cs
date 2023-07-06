using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class SleepController
    {
        [HttpGet]
        [Route("sleep")]
        public async Task<bool> Sleep(int forMs = 1000)
        {
            if (forMs > 0)
                await Task.Delay(forMs);

            return true;
        }

        [HttpGet]
        [Route("wakeup")]
        public async Task<bool> WakeUp(CancellationToken cancellationToken)
        {
            int rested = 0;
            while (rested <= 5000)
            {
                await Task.Delay(100);
                cancellationToken.ThrowIfCancellationRequested();
                rested += 100;
            }

            return true;
        }
    }
}
