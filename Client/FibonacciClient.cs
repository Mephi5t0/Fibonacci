using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using View.Errors;
using View.Fibonacci;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Client
{
    public sealed class FibonacciClient : IFibonacciClient
    {
        private readonly HttpClient httpClient;

        public FibonacciClient()
        {
            this.httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000/")
            };
        }

        public async Task<ClientResult<FibonacciCalculationResult>> CalculateNextNumberAsync(
            FibonacciCalculateInfo calculateInfo,
            CancellationToken token)
        {
            var json = JsonConvert.SerializeObject(calculateInfo);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await this.httpClient.PostAsync("fibonacci/calculate-next", data, token)
                .ConfigureAwait(false);

            var clientResult = new ClientResult<FibonacciCalculationResult> {StatusCode = response.StatusCode};

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync(token).ConfigureAwait(false);
                var calculationResult = JsonSerializer.Deserialize<FibonacciCalculationResult>(content);
                clientResult.Value = calculationResult;
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync(token).ConfigureAwait(false);
                var error = JsonSerializer.Deserialize<Error>(content);
                clientResult.Error = error;
            }

            return clientResult;
        }
    }
}