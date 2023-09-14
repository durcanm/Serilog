using Microsoft.Extensions.Logging;

namespace Lib
{
    public class MathService
    {
        private readonly ILogger<MathService> _logger;

        public MathService(ILogger<MathService> logger)
        {
            _logger = logger;
        }

        public int Sum(int num1, int num2)
        {
            _logger.LogDebug(">>> hello from {serviceName}", nameof(MathService));
            _logger.LogInformation(">>> now summing {num1} and {num2}", num1, num2);

            return
                num1 + num2;
        }
    }
}