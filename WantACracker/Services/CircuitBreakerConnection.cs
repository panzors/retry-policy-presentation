using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WantACracker.Services
{
    public class CircuitBreakerConnection : ConnectionBase
    {
        public CircuitBreakerConnection(HttpClient client, ILogger<CircuitBreakerConnection> logger)
            :base(client, logger)
        {

        }

    }
}
