using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>
        (ILogger<LoggingBehavior<TRequest, TResponse>> logger) // to inject a logger
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("[START] Handle Request={Resquest} - Response={Response} - RequestData={RequestData}", 
                typeof(TRequest).Name, typeof(TResponse).Name, request);

            var timer = new Stopwatch();
            timer.Start();

            var response = await next();

            timer.Stop();
            var timeTaken = timer.Elapsed;
            if (timeTaken.Seconds > 3)
                logger.LogWarning("[PERFORMANCE] The Request={Request} took {TimeTaken} seconds to complete", 
                    typeof(TRequest).Name, timeTaken.TotalSeconds);

            logger.LogInformation("[END] Handle {Request} with {Response}", 
                typeof(TRequest).Name, typeof(TResponse).Name);

            return response;
        }
    }
}
