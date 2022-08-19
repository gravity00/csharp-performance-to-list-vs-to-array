using BenchmarkDotNet.Running;
using PerformanceToListVsToArray.ForeachPerformance;
using PerformanceToListVsToArray.ToPerformance;

BenchmarkRunner.Run<ToListVsToArrayPrimitive>();
BenchmarkRunner.Run<ToListVsToArrayReference>();

BenchmarkRunner.Run<ForeachListVsArrayPrimitive>();
BenchmarkRunner.Run<ForeachListVsArrayReference>();