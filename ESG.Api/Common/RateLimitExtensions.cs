using System.Threading.RateLimiting;

namespace ESG.Api.Common
{
    public static class RateLimitExtensions
    {
        public static IServiceCollection AddApiRateLimiting(
            this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                /* ===============================
                GLOBAL REJECTION HANDLER
                ================================ */
                options.OnRejected = async (context, _) =>
                {
                    var retryAfter = context.Lease.TryGetMetadata(
                        MetadataName.RetryAfter,
                        out var retryAfterTime)
                        ? retryAfterTime
                        : TimeSpan.FromSeconds(60);

                    var retryAtUtc = DateTimeOffset.UtcNow.Add(retryAfter);

                    context.HttpContext.Response.StatusCode =
                        StatusCodes.Status429TooManyRequests;

                    context.HttpContext.Response.Headers["Retry-After"] =
                        retryAtUtc.ToString("R"); // RFC 1123 (HTTP-compliant)

                    await context.HttpContext.Response.WriteAsync(
                        $"Too many requests. Retry at {retryAtUtc:HH:mm:ss} UTC"
                    );
                };
                
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                /* ===============================
                   GLOBAL SAFETY NET (ALL REQUESTS)
                ================================ */
                options.GlobalLimiter =
                    PartitionedRateLimiter.Create<HttpContext, string>(context =>
                        RateLimitPartition.GetFixedWindowLimiter(
                            GetClientKey(context),
                            _ => new FixedWindowRateLimiterOptions
                            {
                                PermitLimit = 20,
                                Window = TimeSpan.FromMinutes(1),
                                QueueLimit = 0
                            }));

                /* ===============================
                   NAMED POLICIES
                ================================ */
                options.AddPolicy(RateLimitPolicies.PublicRead, context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        GetClientKey(context),
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 20,
                            Window = TimeSpan.FromMinutes(1)
                        }));

                options.AddPolicy(RateLimitPolicies.WriteHeavy, context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        GetClientKey(context),
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 1,
                            Window = TimeSpan.FromMinutes(1)
                        }));

                options.AddPolicy(RateLimitPolicies.Sensitive, context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        GetClientKey(context),
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 1,
                            Window = TimeSpan.FromMinutes(1)
                        }));
            });

            return services;
        }

        private static string GetClientKey(HttpContext context)
        {
            return context.User.Identity?.IsAuthenticated == true
                ? context.User.Identity.Name!
                : context.Connection.RemoteIpAddress?.ToString() ?? "anonymous";
        }
    }
}