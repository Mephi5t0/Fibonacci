using System;
using System.Threading;
using System.Threading.Tasks;
using API.Fibonacci;
using API.MessageBus;
using Microsoft.AspNetCore.Mvc;
using View.Errors;
using View.Fibonacci;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FibonacciController : ControllerBase
    {
        private readonly IFibonacciService fibonacciService;
        private readonly IMessageBusService messageBusService;

        public FibonacciController(IFibonacciService fibonacciService, IMessageBusService messageBusService)
        {
            this.fibonacciService = fibonacciService ?? throw new ArgumentNullException(nameof(fibonacciService));
            this.messageBusService = messageBusService ?? throw new ArgumentNullException(nameof(messageBusService));
        }

        [HttpPost]
        [Route("calculate-next")]
        public async Task<ActionResult> CalculateNextAsync(
            [FromBody] FibonacciCalculateInfo calculateInfo,
            CancellationToken token)
        {
            try
            {
                var calculationResult = fibonacciService.CalculateNextFibonacciNumber(calculateInfo);
                messageBusService.SendCalculationResult(calculationResult);
                return this.Ok(calculationResult);
            }
            catch (FibonacciNotValidNumber ex)
            {
                var error = new Error {Message = ex.Message, Code = FibonacciErrorCodes.NotValidFibonacciNumber};
                return this.BadRequest(error);
            }
            catch (FibonacciOverFlowException ex)
            {
                var error = new Error {Message = ex.Message, Code = FibonacciErrorCodes.FibonacciOverFlow};
                return this.BadRequest(error);
            }
        }
    }
}