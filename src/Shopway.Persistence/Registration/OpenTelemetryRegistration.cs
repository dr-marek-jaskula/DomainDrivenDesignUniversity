using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Shopway.Infrastructure.Options;

namespace Microsoft.Extensions.DependencyInjection;

internal static class OpenTelemetryRegistration
{
    internal static IApplicationBuilder UseOpenTelemetry(this IApplicationBuilder app)
    {
        var telemetryOptions = app.GetOptions<OpenTelemetryOptions>();

        if (telemetryOptions.IsLocal() is false)
        {
            app.UseOpenTelemetryPrometheusScrapingEndpoint();
        }

        return app;
    }

    internal static IServiceCollection RegisterOpenTelemetry(this IServiceCollection services, ILoggingBuilder logging, IHostEnvironment environment)
    {
        var openTelemetryOptions = services.GetOptions<OpenTelemetryOptions>();
        bool useOnlyConsoleExporter = openTelemetryOptions.IsLocal();

        services.AddMetrics();

        logging.AddOpenTelemetry(options =>
        {
            options.IncludeScopes = true;
            options.IncludeFormattedMessage = true;

            if (useOnlyConsoleExporter)
            {
                options.AddConsoleExporter();
            }
            else
            {
                options.AddOtlpExporter(options => ConfigureOtlpExporter(options, openTelemetryOptions.OtlpHost));
            }
        });

        services
            .AddOpenTelemetry()
            .AddResources(environment.EnvironmentName, openTelemetryOptions)
            .WithMetrics(metricBuilder =>
            {
                metricBuilder
                    .AddRuntimeInstrumentation()
                    .AddMeter(openTelemetryOptions.Meters);

                if (useOnlyConsoleExporter)
                {
                    metricBuilder.AddConsoleExporter();
                }
                else
                {
                    metricBuilder.AddPrometheusExporter();
                }
            })
            .WithTracing(traceBuilder =>
            {
                if (environment.IsDevelopment())
                {
                    traceBuilder.SetSampler<AlwaysOnSampler>();
                }

                traceBuilder
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation();

                if (useOnlyConsoleExporter)
                {
                    traceBuilder.AddConsoleExporter();
                }
                else
                {
                    traceBuilder.AddOtlpExporter(options => ConfigureOtlpExporter(options, openTelemetryOptions.OtlpHost));
                }
            });

        return services;
    }

    private static IOpenTelemetryBuilder AddResources(this IOpenTelemetryBuilder builder, string environment, OpenTelemetryOptions openTelemetryOptions)
    {
        return builder.ConfigureResource(resourceBuilder => resourceBuilder
            .AddService(serviceName: openTelemetryOptions.ApplicationName, serviceVersion: openTelemetryOptions.Version)
            .AddAttributes(new Dictionary<string, object>
            {
                ["environment.name"] = environment,
                ["team.name"] = openTelemetryOptions.TeamName
            }));
    }

    private static void ConfigureOtlpExporter(this OtlpExporterOptions options, string otlpHost)
    {
        options.Endpoint = new Uri($"http://{otlpHost}:4317");
        options.Protocol = OtlpExportProtocol.Grpc;
    }
}