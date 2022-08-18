using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WantACracker.Services
{
    public class ExponentialBackoffConnection : ConnectionBase
    {
        public ExponentialBackoffConnection(HttpClient client, ILogger<ExponentialBackoffConnection> logger)
            :base(client, logger)
        {

        }
    }
}
