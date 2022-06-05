using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Client;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using View.Errors;
using View.Fibonacci;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Interface
{
    class Program
    {
        private static ConcurrentQueue<long> fibonacciNumbers = new();

        static async Task Main(string[] args)
        {
            var cts = new CancellationTokenSource();

            AppDomain.CurrentDomain.UnhandledException += (_, e) =>
            {
                Console.WriteLine(e.ExceptionObject);
                Console.Read();
                Environment.Exit(1);
            };

            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            const string queue = "fibonacci_calculations";

            var factory = new ConnectionFactory {HostName = "localhost"};
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue, false, false, false, null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (_, eventArgs) =>
            {
                var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                var calculationResult = JsonSerializer.Deserialize<FibonacciCalculationResult>(message);
                fibonacciNumbers.Enqueue(calculationResult!.NextNumber);
                Console.WriteLine($"Got number {calculationResult.NextNumber} from API.");
            };

            channel.BasicConsume(queue, true, consumer);

            var calculationCount = GetCalculationsCount();
            var fibonacciClient = new FibonacciClient();
            var tasks = new List<Task>();
            tasks.AddRange(Enumerable.Range(0, calculationCount)
                .Select(_ => CalculateFibonacciNumbersAsync(fibonacciClient, cts.Token)));
            await (await Task.WhenAny(tasks).ConfigureAwait(false)).ConfigureAwait(false);
        }

        private static async Task CalculateFibonacciNumbersAsync(
            IFibonacciClient fibonacciClient,
            CancellationToken token)
        {
            var calculateInfo = new FibonacciCalculateInfo();
            fibonacciNumbers.Enqueue(0);

            while (!token.IsCancellationRequested)
            {
                if (fibonacciNumbers.TryDequeue(out var fibonacciNumber))
                {
                    calculateInfo.CurrentNumber = fibonacciNumber;
                    var calculateResult = await fibonacciClient.CalculateNextNumberAsync(calculateInfo, token)
                        .ConfigureAwait(false);

                    if (calculateResult.Error?.Code == FibonacciErrorCodes.FibonacciOverFlow)
                    {
                        Console.WriteLine("Can count fibonacci only to long max value.");
                        break;
                    }
                }
                else
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), token).ConfigureAwait(false);
                }
            }
        }

        private static int GetCalculationsCount()
        {
            int calculationCount;

            while (true)
            {
                Console.WriteLine("Enter count of fibonacci calculations: ");
                var input = Console.ReadLine();

                if (!int.TryParse(input, out calculationCount))
                {
                    Console.WriteLine("Calculations count should be integer number");
                }
                else
                {
                    break;
                }
            }

            return calculationCount;
        }
    }
}