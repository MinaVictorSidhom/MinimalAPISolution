namespace MinimalAPI.Endpoint_Filters
{
    public class CustomEndpointFilter : IEndpointFilter
    {
        private readonly ILogger<CustomEndpointFilter> _logger;

        public CustomEndpointFilter(ILogger<CustomEndpointFilter> logger)
        {
            _logger = logger;
        }
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            //Before Logic
            _logger.LogInformation("Endpoint filter - before logic");
           var result= await next(context); // It invokes the subsequent filter or endpoint's request delegate

            //After Logic 
            _logger.LogInformation("Endpoint filter - After logic");

            return result;
        }
    }
}
