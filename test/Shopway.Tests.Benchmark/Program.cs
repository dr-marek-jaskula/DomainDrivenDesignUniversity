using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Shopway.Tests.Performance.Benchmarks;

Console.WriteLine("Hello, World!");
var config = DefaultConfig.Instance;
var summary = BenchmarkRunner.Run<ReflectionUtilitiesBenchmarks>(config);
Console.WriteLine(summary);