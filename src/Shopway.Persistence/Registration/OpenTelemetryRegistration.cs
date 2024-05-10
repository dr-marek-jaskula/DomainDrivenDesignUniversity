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
using System.Diagnostics;

namespace Microsoft.Extensions.DependencyInjection;

internal static class OpenTelemetryRegistration
{
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
                options.AddOtlpExporter(options => ConfigureOtlpCollectorExporter(options, openTelemetryOptions.OtlpCollectorHost));
            }

            options.AddProcessor(new ActivityEventLogProcessor());
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
                    metricBuilder.AddOtlpExporter(options => ConfigureOtlpCollectorExporter(options, openTelemetryOptions.OtlpCollectorHost));
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
                    traceBuilder.AddOtlpExporter(options => ConfigureOtlpCollectorExporter(options, openTelemetryOptions.OtlpCollectorHost));
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

    private static void ConfigureOtlpCollectorExporter(this OtlpExporterOptions options, string otlpCollectorHost)
    {
        const string _grpcCollectorPort = "4317";
        options.Endpoint = new Uri($"http://{otlpCollectorHost}:{_grpcCollectorPort}");
        options.Protocol = OtlpExportProtocol.Grpc;
    }

    private sealed class ActivityEventLogProcessor : BaseProcessor<LogRecord>
    {
        public override void OnEnd(LogRecord log)
        {
            base.OnEnd(log);
            Activity.Current?.AddEvent(new ActivityEvent(log.FormattedMessage!));
        }
    }
}