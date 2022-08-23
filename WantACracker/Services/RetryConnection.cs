using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WantACracker.Services
{
    public class RetryConnection : ConnectionBase
    {
        public RetryConnection(HttpClient client, ILogger<RetryConnection> logger) : base(client, logger)
        {
        }
    }
}
