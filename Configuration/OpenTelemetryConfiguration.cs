using Npgsql;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ERP.Ticketing.HttpApi.Configuration;

public static class OpenTelemetryConfiguration
{
    public static IServiceCollection AddOpenTelemetryService(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .ConfigureResource(builder => builder.AddService(serviceName: "ticketing"))
            .WithTracing(builder =>
            {
                builder.AddOtlpExporter("ticketing", options => options.Endpoint = new Uri("http://127.0.0.1:4317"));
                builder.AddNpgsql();
                builder.AddEntityFrameworkCoreInstrumentation();
                builder.AddAspNetCoreInstrumentation();
            })
            .WithMetrics(builder =>
            {
                builder.AddPrometheusExporter();
                builder.AddRuntimeInstrumentation();
                builder.AddAspNetCoreInstrumentation();
                builder.AddProcessInstrumentation();
                builder.AddEventCountersInstrumentation();
            });
        
        return services;
    }
}