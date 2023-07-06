using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WantACracker.Models;

namespace WantACracker.Services
{
    public abstract class ConnectionBase
    {
        private HttpClient _client;
        private ILogger _logger;

        protected ConnectionBase(HttpClient client, ILogger logger) 
        {
            _client = client;
            _logger = logger;
        }

        public virtual async Task<Cracker> Successful()
        {
            var stopwatch = Stopwatch.StartNew();
            var response = await _client.GetAsync("/allow");
            if (response.IsSuccessStatusCode)
            {
                return new Cracker() 
                { 
                    TimeTaken = stopwatch.ElapsedMilliseconds
                };
            }

            throw new Exception("This shouldn't fail? Or does it?");
        }

        public virtual async Task<Cracker> FiftyFiftyUnsafe(double failureChance = 0.5)
        {
            var stopwatch = Stopwatch.StartNew();
            var response = await _client.GetAsync($"/rng/unsafe?failureChance={failureChance}");
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                return new Cracker()
                {
                    TimeTaken = stopwatch.ElapsedMilliseconds
                };
            }

            throw new Exception("Weird, it should never reach here if unsuccessful");
        }

        public virtual async Task<Cracker> Sleep(int durationMs)
        {
            var stopwatch = Stopwatch.StartNew();
            var response = await _client.GetAsync($"/sleep/sleep?forMs={durationMs}");
            if (response.IsSuccessStatusCode)
            {
                return new Cracker()
                {
                    TimeTaken = stopwatch.ElapsedMilliseconds
                };
            }

            throw new Exception();
        }

        public async Task<bool> WakeUp()
        {
            // because this sleeps for 5 seconds in total, and the client timeout is configured, it should fail
            var response = await _client.GetAsync($"/sleep/wakeup");
            response.EnsureSuccessStatusCode();
            return true;
        }

        public virtual async Task<Cracker> Fail()
        {
            var message = await _client.GetAsync("/fail");
            message.EnsureSuccessStatusCode();
            return null;
        }
    }
}
