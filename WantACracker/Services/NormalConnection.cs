using Microsoft.Extensions.Logging;
using WantACracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace WantACracker.Services
{
    public class NormalConnection : ConnectionBase
    {

        public NormalConnection(HttpClient client, ILogger<NormalConnection> logger) 
            :base(client, logger)
        {
        }
    }
}
