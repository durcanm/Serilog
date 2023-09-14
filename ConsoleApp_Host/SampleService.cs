using Microsoft.Extensions.Logging;

namespace ConsoleApp_Host
{
    public class SampleService
    {
        private readonly ILogger<SampleService> _logger;
        private readonly Options _options;

        public SampleService(ILogger<SampleService> logger, Options options)
        {
            _logger = logger;
            _options = options;
        }

        public void SampleMethod()
        {
            _logger.LogInformation("Hello from {service}, with options: {@options}", nameof(SampleService), _options);
        }
    }
}
