using Polly;
using Polly.CircuitBreaker;
using Polly.Registry;
using Polly.Timeout;
using Polly.Retry;
using System;
namespace Web.Infrastructure.Middlewares
{

    public static class PollyPolicyRegistry
    {
        public static PolicyRegistry GetPolicies()
        {
            var policyRegistry = new PolicyRegistry();

            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        Console.WriteLine($"🔄 Retry {retryCount} due to: {exception.Message}");
                    });

            var timeoutPolicy = Policy.TimeoutAsync(3, TimeoutStrategy.Pessimistic);

            var circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30),
                    onBreak: (ex, time) => Console.WriteLine($"🛑 Circuit broken: {ex.Message}"),
                    onReset: () => Console.WriteLine("✅ Circuit reset"),
                    onHalfOpen: () => Console.WriteLine("⚠️ Circuit in test mode"));

            var retryTimeoutPolicy = retryPolicy.WrapAsync(timeoutPolicy);

            policyRegistry.Add("DatabaseRetryPolicy", retryTimeoutPolicy);
            policyRegistry.Add("CacheRetryPolicy", retryTimeoutPolicy);
            policyRegistry.Add("CircuitBreakerPolicy", circuitBreakerPolicy);

            return policyRegistry;
        }
    }

}
