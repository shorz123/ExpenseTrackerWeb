using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace ExpenseTracker.Api.Extensions;

public static class RateLimitingExtensions
{
    public static IServiceCollection AddExpenseRateLimiting(
        this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode =
                StatusCodes.Status429TooManyRequests;

            options.AddFixedWindowLimiter(
                policyName: "write-policy",
                limiterOptions =>
                {
                    limiterOptions.PermitLimit = 10;
                    limiterOptions.Window = TimeSpan.FromMinutes(1);
                    limiterOptions.QueueLimit = 0;
                    limiterOptions.QueueProcessingOrder =
                        QueueProcessingOrder.OldestFirst;
                    limiterOptions.AutoReplenishment = true;
                });
        });

        return services;
    }
}