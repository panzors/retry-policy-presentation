using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WantACracker.Models;

namespace WantACracker.Services
{
    public interface IRouter : IPaymentService
    {

    }
    public interface IPaymentService
    {
        Task<Receipt> Pay(double failureChance = 0);
    }

    public class Receipt
    {
        public string Processor { get; set; }
    }

    public abstract class SomePay : IPaymentService
    {
        private readonly HttpClient _client;
        private readonly AsyncPolicy _policy;

        public SomePay(HttpClient client, AsyncPolicy policy)
        {
            _client = client;
            _policy = policy;
        }

        public async Task<Receipt> Pay(double failureChance = 0)
        {
            var result = new Receipt() { Processor = this.GetType().ToString() };
            if (_policy == null)
            {
                await Send(failureChance);
                return result;
            }
            await _policy.ExecuteAsync(() => Send(failureChance));
            return result;
        }

        private async Task<Cracker> Send(double failureChance)
        {
            var response = await _client.GetAsync($"/rng/unsafe?failureChance={failureChance}");
            response.EnsureSuccessStatusCode();
            return new Cracker() { };
        }
    }
    public class NoPolicyPay : SomePay
    {
        public NoPolicyPay(HttpClient client, AsyncPolicy policy) : base(client, policy)
        {
        }
    }
    public class CircuitBreakerPay : SomePay
    {
        public CircuitBreakerPay(HttpClient client, AsyncPolicy policy) : base(client, policy)
        {
        }
    }

    public class Router : IRouter
    {
        private readonly Random _r;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<Router> _logger;
        private readonly AsyncCircuitBreakerPolicy _policy;
        private readonly IPaymentService _megaPay;
        private IPaymentService _bigPay;

        public Router(IHttpClientFactory clientFactory, ILogger<Router> logger) 
        {
            _r = new Random();
            _clientFactory = clientFactory;
            _logger = logger;

            _policy = Policy.Handle<HttpRequestException>()
                .AdvancedCircuitBreakerAsync(
                0.5, // 50% of calls success 
                TimeSpan.FromSeconds(5),  // 10 second samples 
                3, //minimum sample size
                TimeSpan.FromSeconds(20), //duration of break
                 OnBreak,
                OnReset);

            var megaPayHttpClient = _clientFactory.CreateClient("megapay");
            megaPayHttpClient.BaseAddress = new Uri("https://localhost:5001/");
            _megaPay = new CircuitBreakerPay(megaPayHttpClient, _policy);

            var bigPayHttpClient = _clientFactory.CreateClient("megapay");
            bigPayHttpClient.BaseAddress = new Uri("https://localhost:5001/");
            
            _bigPay = new NoPolicyPay(bigPayHttpClient, null);
        }

        private void OnReset()
        {
            _logger.LogInformation("Router reset");
        }

        private void OnBreak(Exception arg1, TimeSpan arg2)
        {
            _logger.LogWarning("Router broken");
        }

        public Task<Receipt> Pay(double failureChance = 0)
        {
            if (_policy.CircuitState == CircuitState.Closed)
            {
                if (_r.NextDouble() < 0.5)
                {
                    return Provide<CircuitBreakerPay>().Pay(failureChance);
                }
                return Provide<NoPolicyPay>().Pay(failureChance);
            }
            else if (_policy.CircuitState == CircuitState.Open)
            {
                // this is always going to succeed
                return Provide<NoPolicyPay>().Pay(0);
            }

            // half open state means it's ready to try again
            return Provide<CircuitBreakerPay>().Pay(failureChance);
        }

        private IPaymentService Provide<T>() where T: IPaymentService
        {
            if (typeof(T) == typeof(CircuitBreakerPay))
                return _megaPay;
            return _bigPay;
        }
    }
}
